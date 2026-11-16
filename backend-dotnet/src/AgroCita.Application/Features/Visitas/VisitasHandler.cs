using System.Linq.Expressions;
using AgroCita.Application.Common.Exceptions;
using AgroCita.Application.Common.Interfaces;
using AgroCita.Application.Common.Models;
using AgroCita.Application.Common.Services;
using AgroCita.Domain.Entities;
using FluentValidation;

namespace AgroCita.Application.Features.Visitas;

public class VisitasHandler(
    ActorActual actor,
    IRepository<Visit> visitas,
    IRepository<AssistanceRequest> solicitudes,
    IRepository<Technician> tecnicos,
    IRepository<Producer> productores,
    IRepository<VisitReschedule> reprogramaciones,
    IUnitOfWork unitOfWork,
    IValidator<CrearVisitaDto> crearValidator,
    IValidator<CambiarEstadoVisitaDto> estadoValidator,
    IValidator<ReprogramarVisitaDto> reprogramarValidator)
{
    private static readonly TimeSpan Margen = TimeSpan.FromHours(1);

    public async Task<VisitaDto> CrearAsync(CrearVisitaDto dto, CancellationToken ct)
    {
        await crearValidator.ValidateAndThrowAsync(dto, ct);

        var solicitud = await solicitudes.FirstAsync(
                s => s.Id == dto.SolicitudId, ["Finca.Productor.Usuario"], ct: ct)
            ?? throw Errores.Campo(nameof(dto.SolicitudId), "La solicitud no existe.");

        if (solicitud.Estado is AssistanceRequestStatus.Rechazada or AssistanceRequestStatus.Atendida)
        {
            throw new ConflictException(
                "No se pueden programar visitas para una solicitud rechazada o atendida.");
        }

        var tecnico = await tecnicos.FirstAsync(
                t => t.Id == dto.TecnicoId, ["Usuario"], ct: ct)
            ?? throw Errores.Campo(nameof(dto.TecnicoId), "El técnico no existe.");

        if (!tecnico.Activo)
        {
            throw Errores.Campo(nameof(dto.TecnicoId), "El técnico no está activo.");
        }

        var fecha = dto.FechaProgramada!.Value.AUtc();
        await ExigirAgendaLibreAsync(tecnico.Id, fecha, 0, ct);

        var visita = new Visit
        {
            Solicitud = solicitud,
            Tecnico = tecnico,
            FechaProgramada = fecha,
            Estado = VisitStatus.Pendiente,
            FechaCreacion = Fechas.Ahora
        };

        if (solicitud.Estado == AssistanceRequestStatus.Pendiente)
        {
            solicitud.Estado = AssistanceRequestStatus.EnAtencion;
        }

        visitas.Add(visita);
        await unitOfWork.SaveChangesAsync(ct);

        return visita.ADto();
    }

    public async Task<VisitaDto> CambiarEstadoAsync(
        int id, CambiarEstadoVisitaDto dto, CancellationToken ct)
    {
        await estadoValidator.ValidateAndThrowAsync(dto, ct);

        var visita = await actor.VisitaAsync(id, true, ct, VisitaMapeos.Relaciones);

        if (visita.EstaCerrada)
        {
            throw new ConflictException(
                "La visita ya está completada o cancelada y no admite cambios de estado.");
        }

        visita.Estado = dto.Estado!.Value;
        visita.MotivoCancelacion =
            visita.Estado == VisitStatus.Cancelada ? dto.Motivo?.Trim() : null;

        await unitOfWork.SaveChangesAsync(ct);

        return visita.ADto();
    }

    public async Task<VisitaDto> ReprogramarAsync(
        int id, ReprogramarVisitaDto dto, CancellationToken ct)
    {
        await reprogramarValidator.ValidateAndThrowAsync(dto, ct);

        var visita = await actor.VisitaAsync(id, true, ct, VisitaMapeos.Relaciones);

        if (visita.EstaCerrada)
        {
            throw new ConflictException(
                "No se puede reprogramar una visita completada o cancelada.");
        }

        var nuevaFecha = dto.NuevaFecha!.Value.AUtc();
        await ExigirAgendaLibreAsync(visita.TecnicoId, nuevaFecha, visita.Id, ct);

        reprogramaciones.Add(new VisitReschedule
        {
            VisitaId = visita.Id,
            UsuarioId = actor.UsuarioId,
            FechaAnterior = visita.FechaProgramada,
            FechaNueva = nuevaFecha,
            Motivo = dto.Motivo.Trim(),
            FechaRegistro = Fechas.Ahora
        });

        visita.FechaProgramada = nuevaFecha;
        await unitOfWork.SaveChangesAsync(ct);

        return visita.ADto();
    }

    public async Task<IReadOnlyList<ReprogramacionDto>> ListarReprogramacionesAsync(
        int id, CancellationToken ct)
    {
        if (!await visitas.AnyAsync(v => v.Id == id, ct: ct))
        {
            throw new NotFoundException("La visita no existe.");
        }

        var lista = await reprogramaciones.ListAsync(
            r => r.VisitaId == id,
            q => q.OrderBy(r => r.FechaRegistro),
            incluir: ["Usuario"],
            ct: ct);

        return lista.Select(r => r.ADto()).ToList();
    }

    public async Task<VisitaDetalleDto> ObtenerAsync(int id, CancellationToken ct)
    {
        var visita = await actor.VisitaAsync(id, true, ct, VisitaMapeos.RelacionesDetalle);

        return visita.ADetalleDto();
    }

    public async Task<IReadOnlyList<VisitaDto>> ListarMiasAsync(
        VisitStatus? estado, DateOnly? desde, DateOnly? hasta, CancellationToken ct)
    {
        var tecnicoId = (await actor.TecnicoAsync(ct)).Id;
        var lista = await ListarAsync(Filtro(tecnicoId, estado, desde, hasta), ct);

        return lista.Select(v => v.ADto()).ToList();
    }

    public async Task<IReadOnlyList<VisitaAgendaDto>> AgendaAsync(
        DateOnly? desde, DateOnly? hasta, int? tecnicoId, CancellationToken ct)
    {
        var lista = await ListarAsync(Filtro(tecnicoId, null, desde, hasta), ct);

        return lista.Select(v => v.AAgendaDto()).ToList();
    }

    public async Task<IReadOnlyList<VisitaAgendaDto>> AgendaDeTecnicoAsync(
        int tecnicoId, DateOnly? desde, DateOnly? hasta, CancellationToken ct)
    {
        if (!await tecnicos.AnyAsync(t => t.Id == tecnicoId, ct: ct))
        {
            throw new NotFoundException("El técnico no existe.");
        }

        return await AgendaAsync(desde, hasta, tecnicoId, ct);
    }

    public async Task<IReadOnlyList<VisitaDto>> ListarPorProductorAsync(
        int productorId, CancellationToken ct)
    {
        if (!await productores.AnyAsync(p => p.Id == productorId, ct: ct))
        {
            throw new NotFoundException("El productor no existe.");
        }

        var lista = await ListarAsync(v => v.Solicitud!.Finca!.ProductorId == productorId, ct);

        return lista.Select(v => v.ADto()).ToList();
    }

    private Task<List<Visit>> ListarAsync(
        Expression<Func<Visit, bool>> filtro, CancellationToken ct) =>
        visitas.ListAsync(
            filtro,
            q => q.OrderBy(v => v.FechaProgramada),
            incluir: VisitaMapeos.Relaciones,
            ct: ct);

    private static Expression<Func<Visit, bool>> Filtro(
        int? tecnicoId, VisitStatus? estado, DateOnly? desde, DateOnly? hasta)
    {
        DateTime? inicio = desde?.InicioUtc();
        DateTime? fin = hasta?.AddDays(1).InicioUtc();

        return v =>
            (tecnicoId == null || v.TecnicoId == tecnicoId) &&
            (estado == null || v.Estado == estado) &&
            (inicio == null || v.FechaProgramada >= inicio) &&
            (fin == null || v.FechaProgramada < fin);
    }

    private async Task ExigirAgendaLibreAsync(
        int tecnicoId, DateTime fecha, int visitaId, CancellationToken ct)
    {
        var desde = fecha - Margen;
        var hasta = fecha + Margen;

        if (await visitas.AnyAsync(
                v => v.TecnicoId == tecnicoId &&
                     v.Id != visitaId &&
                     v.Estado != VisitStatus.Cancelada &&
                     v.FechaProgramada > desde &&
                     v.FechaProgramada < hasta,
                ct: ct))
        {
            throw new ConflictException(
                "El técnico ya tiene otra visita programada en ese horario.");
        }
    }
}
