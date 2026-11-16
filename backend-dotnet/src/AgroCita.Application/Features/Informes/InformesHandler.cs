using AgroCita.Application.Common.Exceptions;
using AgroCita.Application.Common.Interfaces;
using AgroCita.Application.Common.Models;
using AgroCita.Application.Common.Services;
using AgroCita.Application.Features.Calidad;
using AgroCita.Application.Features.Fertilizacion;
using AgroCita.Application.Features.Fincas;
using AgroCita.Application.Features.Visitas;
using AgroCita.Domain.Entities;
using FluentValidation;

namespace AgroCita.Application.Features.Informes;

public class InformesHandler(
    ActorActual actor,
    IRepository<Report> informes,
    IRepository<Visit> visitas,
    IRepository<Inspection> inspecciones,
    IRepository<FertilizationRecommendation> recomendaciones,
    IRepository<FertilizerApplication> aplicaciones,
    IRepository<Farm> fincas,
    IRepository<Crop> cultivos,
    IUnitOfWork unitOfWork,
    IValidator<CrearInformeDto> validator)
{
    public async Task<InformeDto> CrearAsync(int visitaId, CrearInformeDto dto, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(dto, ct);

        var visita = await actor.VisitaAsync(visitaId, true, ct, "Tecnico.Usuario", "Informe");

        if (visita.Estado is not (VisitStatus.EnProceso or VisitStatus.Completada))
        {
            throw new ConflictException(
                "Solo se elabora el informe de una visita en proceso o completada.");
        }

        if (visita.Informe is not null)
        {
            throw new ConflictException("La visita ya tiene un informe.");
        }

        var informe = new Report
        {
            VisitaId = visita.Id,
            Observaciones = dto.Observaciones.Trim(),
            Diagnostico = dto.Diagnostico.Trim(),
            Recomendaciones = dto.Recomendaciones.Trim(),
            FechaElaboracion = Fechas.Ahora
        };

        informes.Add(informe);
        await unitOfWork.SaveChangesAsync(ct);

        return informe.ADto(visita, await InspeccionesDeVisitaAsync(visitaId, ct));
    }

    public async Task<InformeDto> ObtenerAsync(int visitaId, CancellationToken ct)
    {
        var visita = await actor.VisitaAsync(visitaId, false, ct, "Tecnico.Usuario", "Informe");

        var informe = visita.Informe
            ?? throw new NotFoundException("La visita todavía no tiene informe.");

        return informe.ADto(visita, await InspeccionesDeVisitaAsync(visitaId, ct));
    }

    public async Task<IReadOnlyList<InformeResumenDto>> ListarMiosAsync(int? fincaId, CancellationToken ct)
    {
        var productorId = (await actor.ProductorAsync(ct)).Id;

        var lista = await informes.ListAsync(
            i => i.Visita!.Solicitud!.Finca!.ProductorId == productorId &&
                 (fincaId == null || i.Visita.Solicitud.FincaId == fincaId),
            q => q.OrderByDescending(i => i.FechaElaboracion),
            incluir: InformeMapeos.RelacionesResumen,
            ct: ct);

        return lista.Select(i => i.AResumenDto()).ToList();
    }

    public async Task<HistorialDto> HistorialAsync(
        int? productorId, int? fincaId, int? cultivoId, CancellationToken ct)
    {
        if (actor.EsProductor)
        {
            productorId = (await actor.ProductorAsync(ct)).Id;
        }

        var finca = fincaId is null
            ? null
            : await fincas.FirstAsync(
                f => f.Id == fincaId && (productorId == null || f.ProductorId == productorId),
                ct: ct);

        var cultivo = cultivoId is null
            ? null
            : await cultivos.FirstAsync(
                c => c.Id == cultivoId && (productorId == null || c.Finca!.ProductorId == productorId),
                ct: ct);

        var listaVisitas = await visitas.ListAsync(
            v => (productorId == null || v.Solicitud!.Finca!.ProductorId == productorId) &&
                 (fincaId == null || v.Solicitud!.FincaId == fincaId) &&
                 (cultivoId == null || v.Solicitud!.Cultivos.Any(c => c.CultivoId == cultivoId)),
            q => q.OrderByDescending(v => v.FechaProgramada),
            incluir: VisitaMapeos.Relaciones,
            ct: ct);

        var listaInspecciones = await inspecciones.ListAsync(
            i => (productorId == null || i.Cultivo!.Finca!.ProductorId == productorId) &&
                 (fincaId == null || i.Cultivo!.FincaId == fincaId) &&
                 (cultivoId == null || i.CultivoId == cultivoId),
            q => q.OrderByDescending(i => i.FechaInspeccion),
            incluir: CalidadMapeos.Relaciones,
            ct: ct);

        var listaRecomendaciones = await recomendaciones.ListAsync(
            r => (productorId == null || r.Cultivo!.Finca!.ProductorId == productorId) &&
                 (fincaId == null || r.Cultivo!.FincaId == fincaId) &&
                 (cultivoId == null || r.CultivoId == cultivoId),
            q => q.OrderByDescending(r => r.FechaRecomendacion),
            incluir: FertilizacionMapeos.RelacionesRecomendacion,
            ct: ct);

        var listaAplicaciones = await aplicaciones.ListAsync(
            a => (productorId == null || a.Cultivo!.Finca!.ProductorId == productorId) &&
                 (fincaId == null || a.Cultivo!.FincaId == fincaId) &&
                 (cultivoId == null || a.CultivoId == cultivoId),
            q => q.OrderByDescending(a => a.FechaAplicacion),
            incluir: FertilizacionMapeos.RelacionesAplicacion,
            ct: ct);

        return new HistorialDto(
            finca?.AReferencia(),
            cultivo?.AReferencia(),
            listaVisitas.Select(v => v.ADto()).ToList(),
            listaInspecciones.Select(i => i.ADto()).ToList(),
            listaRecomendaciones.Select(r => r.ADto()).ToList(),
            listaAplicaciones.Select(a => a.ADto()).ToList());
    }

    private Task<List<Inspection>> InspeccionesDeVisitaAsync(int visitaId, CancellationToken ct) =>
        inspecciones.ListAsync(
            i => i.VisitaId == visitaId,
            q => q.OrderBy(i => i.Id),
            incluir: CalidadMapeos.Relaciones,
            ct: ct);
}
