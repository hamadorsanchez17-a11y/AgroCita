using AgroCita.Application.Common.Exceptions;
using AgroCita.Application.Common.Interfaces;
using AgroCita.Application.Common.Models;
using AgroCita.Application.Common.Services;
using AgroCita.Domain.Entities;
using FluentValidation;

namespace AgroCita.Application.Features.Solicitudes;

public class SolicitudesHandler(
    ActorActual actor,
    IRepository<AssistanceRequest> solicitudes,
    IRepository<Farm> fincas,
    IUnitOfWork unitOfWork,
    IValidator<CrearSolicitudDto> crearValidator,
    IValidator<CambiarEstadoSolicitudDto> estadoValidator)
{
    public async Task<SolicitudDto> CrearAsync(CrearSolicitudDto dto, CancellationToken ct)
    {
        await crearValidator.ValidateAndThrowAsync(dto, ct);

        var productorId = (await actor.ProductorAsync(ct)).Id;

        var finca = await fincas.FirstAsync(
                f => f.Id == dto.FincaId && f.ProductorId == productorId,
                ["Cultivos", "Productor.Usuario"],
                ct: ct)
            ?? throw Errores.Campo(nameof(dto.FincaId), "La finca no existe.");

        var cultivos = dto.CultivoIds!
            .Distinct()
            .Select(id => finca.Cultivos.FirstOrDefault(c => c.Id == id))
            .ToList();

        if (cultivos.Any(c => c is null))
        {
            throw Errores.Campo(
                nameof(dto.CultivoIds),
                "Todos los cultivos deben pertenecer a la finca seleccionada.");
        }

        var solicitud = new AssistanceRequest
        {
            Finca = finca,
            FechaDeseada = dto.FechaDeseada!.Value,
            Motivo = dto.Motivo.Trim(),
            Estado = AssistanceRequestStatus.Pendiente,
            FechaCreacion = Fechas.Ahora,
            Cultivos = cultivos
                .Select(c => new AssistanceRequestCrop { Cultivo = c })
                .ToList()
        };

        solicitudes.Add(solicitud);
        await unitOfWork.SaveChangesAsync(ct);

        return solicitud.ADto();
    }

    public async Task<IReadOnlyList<SolicitudDto>> ListarMiasAsync(
        AssistanceRequestStatus? estado, CancellationToken ct)
    {
        var productorId = (await actor.ProductorAsync(ct)).Id;

        var lista = await solicitudes.ListAsync(
            s => s.Finca!.ProductorId == productorId && (estado == null || s.Estado == estado),
            q => q.OrderByDescending(s => s.FechaCreacion),
            incluir: SolicitudMapeos.Relaciones,
            ct: ct);

        return lista.Select(s => s.ADto()).ToList();
    }

    public async Task<PaginadoDto<SolicitudDto>> ListarAsync(
        AssistanceRequestStatus? estado,
        DateOnly? desde,
        DateOnly? hasta,
        int? pagina,
        int? tamano,
        CancellationToken ct)
    {
        var (numeroPagina, tamanoPagina, saltar) = Paginacion.Normalizar(pagina, tamano);

        System.Linq.Expressions.Expression<Func<AssistanceRequest, bool>> filtro = s =>
            (estado == null || s.Estado == estado) &&
            (desde == null || s.FechaDeseada >= desde) &&
            (hasta == null || s.FechaDeseada <= hasta);

        var total = await solicitudes.CountAsync(filtro, ct: ct);

        var lista = await solicitudes.ListAsync(
            filtro,
            q => q.OrderByDescending(s => s.FechaCreacion),
            saltar,
            tamanoPagina,
            SolicitudMapeos.Relaciones,
            ct: ct);

        return new PaginadoDto<SolicitudDto>(
            lista.Select(s => s.ADto()).ToList(),
            total,
            numeroPagina,
            tamanoPagina);
    }

    public async Task<SolicitudDetalleDto> ObtenerAsync(int id, CancellationToken ct)
    {
        var solicitud = await solicitudes.FirstAsync(
                s => s.Id == id, SolicitudMapeos.RelacionesDetalle, ct: ct)
            ?? throw new NotFoundException("La solicitud no existe.");

        if (actor.EsProductor &&
            solicitud.Finca!.ProductorId != (await actor.ProductorAsync(ct)).Id)
        {
            throw new NotFoundException("La solicitud no existe.");
        }

        return solicitud.ADetalleDto();
    }

    public async Task<SolicitudDto> CambiarEstadoAsync(
        int id, CambiarEstadoSolicitudDto dto, CancellationToken ct)
    {
        await estadoValidator.ValidateAndThrowAsync(dto, ct);

        var solicitud = await solicitudes.FirstAsync(
                s => s.Id == id, SolicitudMapeos.Relaciones, ct: ct)
            ?? throw new NotFoundException("La solicitud no existe.");

        solicitud.Estado = dto.Estado!.Value;
        solicitud.ComentarioEstado = dto.Comentario?.Trim();

        await unitOfWork.SaveChangesAsync(ct);

        return solicitud.ADto();
    }
}
