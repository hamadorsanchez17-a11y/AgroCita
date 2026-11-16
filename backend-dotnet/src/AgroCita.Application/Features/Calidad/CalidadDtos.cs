using AgroCita.Application.Common.Models;
using AgroCita.Application.Features.Fincas;
using AgroCita.Application.Features.Tecnicos;
using AgroCita.Domain.Entities;
using FluentValidation;

namespace AgroCita.Application.Features.Calidad;

public record ParametroRequestDto(
    string Nombre,
    string? Unidad,
    decimal? ValorMinimo,
    decimal? ValorMaximo);

public record ParametroEvaluacionDto(
    int Id,
    int TipoCultivoId,
    string Nombre,
    string? Unidad,
    decimal? ValorMinimo,
    decimal? ValorMaximo);

public record CategoriaRequestDto(string Nombre, string? Descripcion);

public record CategoriaCalidadDto(int Id, string Nombre, string? Descripcion);

public record ResultadoRequestDto(int ParametroId, string Valor);

public record CrearInspeccionDto(
    int CultivoId,
    DateTime? Fecha,
    int? CategoriaCalidadId,
    string? Observaciones,
    List<ResultadoRequestDto>? Resultados);

public record ResultadoDto(int ParametroId, string Parametro, string? Unidad, string Valor);

public record InspeccionDto(
    int Id,
    int VisitaId,
    ReferenciaDto Cultivo,
    ReferenciaDto Tecnico,
    DateTime Fecha,
    ReferenciaDto? CategoriaCalidad,
    string? Observaciones,
    IReadOnlyList<ResultadoDto> Resultados);

public static class CalidadMapeos
{
    public static readonly string[] Relaciones =
    [
        "Visita.Tecnico.Usuario",
        "Cultivo.Finca",
        "CategoriaCalidad",
        "Resultados.ParametroEvaluacion"
    ];

    public static ParametroEvaluacionDto ADto(this EvaluationParameter parametro) => new(
        parametro.Id,
        parametro.TipoCultivoId,
        parametro.Nombre,
        parametro.Unidad,
        parametro.ValorMinimo,
        parametro.ValorMaximo);

    public static CategoriaCalidadDto ADto(this QualityCategory categoria) =>
        new(categoria.Id, categoria.Nombre, categoria.Descripcion);

    public static InspeccionDto ADto(this Inspection inspeccion) => new(
        inspeccion.Id,
        inspeccion.VisitaId,
        inspeccion.Cultivo?.AReferencia() ?? new ReferenciaDto(inspeccion.CultivoId, string.Empty),
        inspeccion.Visita?.Tecnico?.AReferencia() ?? new ReferenciaDto(0, string.Empty),
        inspeccion.FechaInspeccion,
        inspeccion.CategoriaCalidad is { } categoria
            ? new ReferenciaDto(categoria.Id, categoria.Nombre)
            : null,
        inspeccion.Observaciones,
        inspeccion.Resultados
            .Select(r => new ResultadoDto(
                r.ParametroEvaluacionId,
                r.ParametroEvaluacion?.Nombre ?? string.Empty,
                r.ParametroEvaluacion?.Unidad,
                r.Valor))
            .ToList());
}

public class ParametroRequestValidator : AbstractValidator<ParametroRequestDto>
{
    public ParametroRequestValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del parámetro es obligatorio.")
            .MaximumLength(80).WithMessage("El nombre no puede superar los 80 caracteres.");

        RuleFor(x => x.Unidad)
            .MaximumLength(20).WithMessage("La unidad no puede superar los 20 caracteres.");

        RuleFor(x => x.ValorMaximo)
            .GreaterThanOrEqualTo(x => x.ValorMinimo)
            .WithMessage("El valor máximo debe ser mayor o igual que el mínimo.")
            .When(x => x.ValorMinimo is not null && x.ValorMaximo is not null);
    }
}

public class CategoriaRequestValidator : AbstractValidator<CategoriaRequestDto>
{
    public CategoriaRequestValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre de la categoría es obligatorio.")
            .MaximumLength(60).WithMessage("El nombre no puede superar los 60 caracteres.");

        RuleFor(x => x.Descripcion)
            .MaximumLength(250).WithMessage("La descripción no puede superar los 250 caracteres.");
    }
}

public class CrearInspeccionValidator : AbstractValidator<CrearInspeccionDto>
{
    public CrearInspeccionValidator()
    {
        RuleFor(x => x.CultivoId)
            .GreaterThan(0).WithMessage("El cultivo es obligatorio.");

        RuleFor(x => x.Fecha)
            .Must(f => f is null || f.Value.AUtc() <= Fechas.Ahora.AddMinutes(5))
            .WithMessage("La fecha de la inspección no puede ser futura.");

        RuleFor(x => x.Resultados)
            .NotEmpty().WithMessage("Debe registrar al menos un resultado.")
            .Must(r => r is null || r.Select(x => x.ParametroId).Distinct().Count() == r.Count)
            .WithMessage("No se puede repetir un parámetro en la misma inspección.");

        RuleForEach(x => x.Resultados).ChildRules(resultado =>
        {
            resultado.RuleFor(r => r.ParametroId)
                .GreaterThan(0).WithMessage("El parámetro es obligatorio.");

            resultado.RuleFor(r => r.Valor)
                .NotEmpty().WithMessage("El valor del resultado es obligatorio.")
                .MaximumLength(100).WithMessage("El valor no puede superar los 100 caracteres.");
        });
    }
}
