using AgroCita.Application.Common.Exceptions;
using AgroCita.Application.Common.Interfaces;
using AgroCita.Application.Common.Models;
using AgroCita.Application.Common.Services;
using AgroCita.Domain.Entities;
using FluentValidation;

namespace AgroCita.Application.Features.Calidad;

public class CalidadHandler(
    ActorActual actor,
    IRepository<CropType> tipos,
    IRepository<EvaluationParameter> parametros,
    IRepository<QualityCategory> categorias,
    IRepository<Inspection> inspecciones,
    IUnitOfWork unitOfWork,
    IValidator<ParametroRequestDto> parametroValidator,
    IValidator<CategoriaRequestDto> categoriaValidator,
    IValidator<CrearInspeccionDto> inspeccionValidator)
{
    public async Task<IReadOnlyList<ParametroEvaluacionDto>> ListarParametrosAsync(
        int tipoCultivoId, CancellationToken ct)
    {
        await ExigirTipoAsync(tipoCultivoId, ct);

        var lista = await parametros.ListAsync(
            p => p.TipoCultivoId == tipoCultivoId,
            q => q.OrderBy(p => p.Nombre),
            ct: ct);

        return lista.Select(p => p.ADto()).ToList();
    }

    public async Task<ParametroEvaluacionDto> CrearParametroAsync(
        int tipoCultivoId, ParametroRequestDto dto, CancellationToken ct)
    {
        await parametroValidator.ValidateAndThrowAsync(dto, ct);
        await ExigirTipoAsync(tipoCultivoId, ct);

        var nombre = dto.Nombre.Trim();
        var nombreMinusculas = nombre.ToLower();

        if (await parametros.AnyAsync(
                p => p.TipoCultivoId == tipoCultivoId && p.Nombre.ToLower() == nombreMinusculas,
                ct: ct))
        {
            throw new ConflictException(
                "Ya existe un parámetro con ese nombre para el tipo de cultivo.");
        }

        var parametro = new EvaluationParameter
        {
            TipoCultivoId = tipoCultivoId,
            Nombre = nombre,
            Unidad = dto.Unidad?.Trim(),
            ValorMinimo = dto.ValorMinimo,
            ValorMaximo = dto.ValorMaximo
        };

        parametros.Add(parametro);
        await unitOfWork.SaveChangesAsync(ct);

        return parametro.ADto();
    }

    public async Task<IReadOnlyList<CategoriaCalidadDto>> ListarCategoriasAsync(CancellationToken ct)
    {
        var lista = await categorias.ListAsync(ordenar: q => q.OrderBy(c => c.Nombre), ct: ct);

        return lista.Select(c => c.ADto()).ToList();
    }

    public async Task<CategoriaCalidadDto> CrearCategoriaAsync(
        CategoriaRequestDto dto, CancellationToken ct)
    {
        await categoriaValidator.ValidateAndThrowAsync(dto, ct);

        var nombre = dto.Nombre.Trim();
        var nombreMinusculas = nombre.ToLower();

        if (await categorias.AnyAsync(c => c.Nombre.ToLower() == nombreMinusculas, ct: ct))
        {
            throw new ConflictException("Ya existe una categoría de calidad con ese nombre.");
        }

        var categoria = new QualityCategory
        {
            Nombre = nombre,
            Descripcion = dto.Descripcion?.Trim()
        };

        categorias.Add(categoria);
        await unitOfWork.SaveChangesAsync(ct);

        return categoria.ADto();
    }

    public async Task<InspeccionDto> CrearInspeccionAsync(
        int visitaId, CrearInspeccionDto dto, CancellationToken ct)
    {
        await inspeccionValidator.ValidateAndThrowAsync(dto, ct);

        var visita = await actor.VisitaAsync(
            visitaId, true, ct, "Tecnico.Usuario", "Solicitud.Cultivos.Cultivo.Finca");

        if (visita.Estado is not (VisitStatus.EnProceso or VisitStatus.Completada))
        {
            throw new ConflictException(
                "Solo se registran inspecciones en visitas en proceso o completadas.");
        }

        var cultivo = visita.Solicitud!.Cultivos
                .FirstOrDefault(c => c.CultivoId == dto.CultivoId)?.Cultivo
            ?? throw Errores.Campo(
                nameof(dto.CultivoId),
                "El cultivo no está incluido en la solicitud de la visita.");

        if (await inspecciones.AnyAsync(
                i => i.VisitaId == visitaId && i.CultivoId == dto.CultivoId, ct: ct))
        {
            throw new ConflictException(
                "Ya existe una inspección de ese cultivo en esta visita.");
        }

        QualityCategory? categoria = null;

        if (dto.CategoriaCalidadId is int categoriaId)
        {
            categoria = await categorias.FirstAsync(c => c.Id == categoriaId, ct: ct)
                ?? throw Errores.Campo(
                    nameof(dto.CategoriaCalidadId),
                    "La categoría de calidad no existe.");
        }

        var tipoCultivoId = cultivo.TipoCultivoId;
        var disponibles = await parametros.ListAsync(
            p => p.TipoCultivoId == tipoCultivoId, ct: ct);

        var resultados = new List<ParameterResult>();

        foreach (var resultado in dto.Resultados!)
        {
            var parametro = disponibles.FirstOrDefault(p => p.Id == resultado.ParametroId)
                ?? throw Errores.Campo(
                    nameof(dto.Resultados),
                    "Todos los parámetros deben pertenecer al tipo de cultivo inspeccionado.");

            resultados.Add(new ParameterResult
            {
                ParametroEvaluacion = parametro,
                Valor = resultado.Valor.Trim()
            });
        }

        var inspeccion = new Inspection
        {
            Visita = visita,
            Cultivo = cultivo,
            CategoriaCalidad = categoria,
            FechaInspeccion = dto.Fecha?.AUtc() ?? Fechas.Ahora,
            Observaciones = dto.Observaciones?.Trim(),
            Resultados = resultados
        };

        inspecciones.Add(inspeccion);
        await unitOfWork.SaveChangesAsync(ct);

        return inspeccion.ADto();
    }

    public async Task<InspeccionDto> ObtenerInspeccionAsync(int id, CancellationToken ct)
    {
        var inspeccion = await inspecciones.FirstAsync(
                i => i.Id == id, CalidadMapeos.Relaciones, ct: ct)
            ?? throw new NotFoundException("La inspección no existe.");

        if (actor.EsProductor &&
            inspeccion.Cultivo!.Finca!.ProductorId != (await actor.ProductorAsync(ct)).Id)
        {
            throw new NotFoundException("La inspección no existe.");
        }

        return inspeccion.ADto();
    }

    public async Task<IReadOnlyList<InspeccionDto>> ListarPorCultivoAsync(
        int cultivoId, CancellationToken ct)
    {
        await actor.CultivoAsync(cultivoId, ct);

        var lista = await inspecciones.ListAsync(
            i => i.CultivoId == cultivoId,
            q => q.OrderByDescending(i => i.FechaInspeccion),
            incluir: CalidadMapeos.Relaciones,
            ct: ct);

        return lista.Select(i => i.ADto()).ToList();
    }

    private async Task ExigirTipoAsync(int tipoCultivoId, CancellationToken ct)
    {
        if (!await tipos.AnyAsync(t => t.Id == tipoCultivoId, ct: ct))
        {
            throw new NotFoundException("El tipo de cultivo no existe.");
        }
    }
}
