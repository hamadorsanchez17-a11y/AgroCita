using AgroCita.Application.Common.Exceptions;
using AgroCita.Application.Common.Interfaces;
using AgroCita.Application.Common.Models;
using AgroCita.Application.Common.Services;
using AgroCita.Domain.Entities;
using FluentValidation;

namespace AgroCita.Application.Features.Fertilizacion;

public class FertilizacionHandler(
    ActorActual actor,
    IRepository<Fertilizer> fertilizantes,
    IRepository<FertilizationRecommendation> recomendaciones,
    IRepository<FertilizerApplication> aplicaciones,
    IRepository<Visit> visitas,
    IUnitOfWork unitOfWork,
    IValidator<FertilizanteRequestDto> fertilizanteValidator,
    IValidator<CrearRecomendacionDto> recomendacionValidator,
    IValidator<CrearAplicacionDto> aplicacionValidator)
{
    public async Task<IReadOnlyList<FertilizanteDto>> ListarFertilizantesAsync(CancellationToken ct)
    {
        var lista = await fertilizantes.ListAsync(ordenar: q => q.OrderBy(f => f.Nombre), ct: ct);

        return lista.Select(f => f.ADto()).ToList();
    }

    public async Task<FertilizanteDto> CrearFertilizanteAsync(
        FertilizanteRequestDto dto, CancellationToken ct)
    {
        await fertilizanteValidator.ValidateAndThrowAsync(dto, ct);

        var nombre = dto.Nombre.Trim();
        var nombreMinusculas = nombre.ToLower();

        if (await fertilizantes.AnyAsync(f => f.Nombre.ToLower() == nombreMinusculas, ct: ct))
        {
            throw new ConflictException("Ya existe un fertilizante con ese nombre.");
        }

        var fertilizante = new Fertilizer
        {
            Nombre = nombre,
            Tipo = dto.Tipo.Trim(),
            UnidadMedida = dto.UnidadMedida.Trim(),
            Composicion = dto.Composicion?.Trim()
        };

        fertilizantes.Add(fertilizante);
        await unitOfWork.SaveChangesAsync(ct);

        return fertilizante.ADto();
    }

    public async Task<RecomendacionDto> CrearRecomendacionAsync(
        int cultivoId, CrearRecomendacionDto dto, CancellationToken ct)
    {
        await recomendacionValidator.ValidateAndThrowAsync(dto, ct);

        var cultivo = await actor.CultivoAsync(cultivoId, ct);
        var tecnicoId = (await actor.TecnicoAsync(ct)).Id;

        var visita = await visitas.FirstAsync(
            v => v.Id == dto.VisitaId && v.TecnicoId == tecnicoId,
            ["Tecnico.Usuario", "Solicitud.Cultivos"],
            ct: ct);

        if (visita is null || visita.Solicitud!.Cultivos.All(c => c.CultivoId != cultivo.Id))
        {
            throw Errores.Campo(
                nameof(dto.VisitaId),
                "La visita no existe, no está asignada al técnico o no incluye el cultivo.");
        }

        var fertilizante = await FertilizanteAsync(dto.FertilizanteId, ct);

        var recomendacion = new FertilizationRecommendation
        {
            Visita = visita,
            CultivoId = cultivo.Id,
            Fertilizante = fertilizante,
            DosisRecomendada = dto.Dosis,
            Frecuencia = dto.Frecuencia?.Trim(),
            Indicaciones = dto.Indicaciones.Trim(),
            FechaRecomendacion = dto.Fecha ?? Fechas.Hoy
        };

        recomendaciones.Add(recomendacion);
        await unitOfWork.SaveChangesAsync(ct);

        return recomendacion.ADto();
    }

    public async Task<IReadOnlyList<RecomendacionDto>> ListarRecomendacionesAsync(
        int cultivoId, CancellationToken ct)
    {
        await actor.CultivoAsync(cultivoId, ct);

        var lista = await recomendaciones.ListAsync(
            r => r.CultivoId == cultivoId,
            q => q.OrderByDescending(r => r.FechaRecomendacion).ThenByDescending(r => r.Id),
            incluir: FertilizacionMapeos.RelacionesRecomendacion,
            ct: ct);

        return lista.Select(r => r.ADto()).ToList();
    }

    public async Task<AplicacionDto> CrearAplicacionAsync(
        int cultivoId, CrearAplicacionDto dto, CancellationToken ct)
    {
        await aplicacionValidator.ValidateAndThrowAsync(dto, ct);

        var cultivo = await actor.CultivoAsync(cultivoId, ct);
        var tecnico = await actor.TecnicoAsync(ct);
        var fertilizante = await FertilizanteAsync(dto.FertilizanteId, ct);

        if (dto.RecomendacionId is int recomendacionId &&
            !await recomendaciones.AnyAsync(
                r => r.Id == recomendacionId && r.CultivoId == cultivoId, ct: ct))
        {
            throw Errores.Campo(
                nameof(dto.RecomendacionId),
                "La recomendación no existe para este cultivo.");
        }

        var aplicacion = new FertilizerApplication
        {
            CultivoId = cultivo.Id,
            Fertilizante = fertilizante,
            RecomendacionId = dto.RecomendacionId,
            Tecnico = tecnico,
            CantidadAplicada = dto.Cantidad,
            FechaAplicacion = dto.FechaAplicacion!.Value,
            Observaciones = dto.Observaciones?.Trim()
        };

        aplicaciones.Add(aplicacion);
        await unitOfWork.SaveChangesAsync(ct);

        return aplicacion.ADto();
    }

    public async Task<IReadOnlyList<AplicacionDto>> ListarAplicacionesAsync(
        int cultivoId, CancellationToken ct)
    {
        await actor.CultivoAsync(cultivoId, ct);

        var lista = await aplicaciones.ListAsync(
            a => a.CultivoId == cultivoId,
            q => q.OrderByDescending(a => a.FechaAplicacion).ThenByDescending(a => a.Id),
            incluir: FertilizacionMapeos.RelacionesAplicacion,
            ct: ct);

        return lista.Select(a => a.ADto()).ToList();
    }

    private async Task<Fertilizer> FertilizanteAsync(int id, CancellationToken ct) =>
        await fertilizantes.FirstAsync(f => f.Id == id, ct: ct)
        ?? throw Errores.Campo("FertilizanteId", "El fertilizante no existe.");
}
