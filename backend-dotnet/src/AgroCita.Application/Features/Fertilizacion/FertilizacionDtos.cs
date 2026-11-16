using AgroCita.Application.Common.Models;
using AgroCita.Application.Features.Tecnicos;
using AgroCita.Domain.Entities;
using FluentValidation;

namespace AgroCita.Application.Features.Fertilizacion;

public record FertilizanteRequestDto(
    string Nombre,
    string Tipo,
    string UnidadMedida,
    string? Composicion);

public record FertilizanteDto(
    int Id,
    string Nombre,
    string Tipo,
    string UnidadMedida,
    string? Composicion);

public record CrearRecomendacionDto(
    int VisitaId,
    int FertilizanteId,
    decimal Dosis,
    string? Frecuencia,
    string Indicaciones,
    DateOnly? Fecha);

public record RecomendacionDto(
    int Id,
    int CultivoId,
    int VisitaId,
    ReferenciaDto Fertilizante,
    decimal Dosis,
    string Unidad,
    string? Frecuencia,
    string Indicaciones,
    DateOnly Fecha,
    ReferenciaDto Tecnico);

public record CrearAplicacionDto(
    int? RecomendacionId,
    int FertilizanteId,
    decimal Cantidad,
    DateOnly? FechaAplicacion,
    string? Observaciones);

public record AplicacionDto(
    int Id,
    int CultivoId,
    int? RecomendacionId,
    ReferenciaDto Fertilizante,
    decimal Cantidad,
    string Unidad,
    DateOnly FechaAplicacion,
    string? Observaciones,
    ReferenciaDto Tecnico);

public static class FertilizacionMapeos
{
    public static readonly string[] RelacionesRecomendacion =
        ["Fertilizante", "Visita.Tecnico.Usuario"];

    public static readonly string[] RelacionesAplicacion =
        ["Fertilizante", "Tecnico.Usuario"];

    public static FertilizanteDto ADto(this Fertilizer fertilizante) => new(
        fertilizante.Id,
        fertilizante.Nombre,
        fertilizante.Tipo,
        fertilizante.UnidadMedida,
        fertilizante.Composicion);

    public static RecomendacionDto ADto(this FertilizationRecommendation recomendacion) => new(
        recomendacion.Id,
        recomendacion.CultivoId,
        recomendacion.VisitaId,
        new ReferenciaDto(recomendacion.FertilizanteId, recomendacion.Fertilizante?.Nombre ?? string.Empty),
        recomendacion.DosisRecomendada,
        recomendacion.Fertilizante?.UnidadMedida ?? string.Empty,
        recomendacion.Frecuencia,
        recomendacion.Indicaciones,
        recomendacion.FechaRecomendacion,
        recomendacion.Visita?.Tecnico?.AReferencia() ?? new ReferenciaDto(0, string.Empty));

    public static AplicacionDto ADto(this FertilizerApplication aplicacion) => new(
        aplicacion.Id,
        aplicacion.CultivoId,
        aplicacion.RecomendacionId,
        new ReferenciaDto(aplicacion.FertilizanteId, aplicacion.Fertilizante?.Nombre ?? string.Empty),
        aplicacion.CantidadAplicada,
        aplicacion.Fertilizante?.UnidadMedida ?? string.Empty,
        aplicacion.FechaAplicacion,
        aplicacion.Observaciones,
        aplicacion.Tecnico?.AReferencia() ?? new ReferenciaDto(aplicacion.TecnicoId, string.Empty));
}

public class FertilizanteRequestValidator : AbstractValidator<FertilizanteRequestDto>
{
    public FertilizanteRequestValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del fertilizante es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(x => x.Tipo)
            .NotEmpty().WithMessage("El tipo de fertilizante es obligatorio.")
            .MaximumLength(40).WithMessage("El tipo no puede superar los 40 caracteres.");

        RuleFor(x => x.UnidadMedida)
            .NotEmpty().WithMessage("La unidad de medida es obligatoria.")
            .MaximumLength(20).WithMessage("La unidad de medida no puede superar los 20 caracteres.");

        RuleFor(x => x.Composicion)
            .MaximumLength(250).WithMessage("La composición no puede superar los 250 caracteres.");
    }
}

public class CrearRecomendacionValidator : AbstractValidator<CrearRecomendacionDto>
{
    public CrearRecomendacionValidator()
    {
        RuleFor(x => x.VisitaId)
            .GreaterThan(0).WithMessage("La visita es obligatoria.");

        RuleFor(x => x.FertilizanteId)
            .GreaterThan(0).WithMessage("El fertilizante es obligatorio.");

        RuleFor(x => x.Dosis)
            .GreaterThan(0).WithMessage("La dosis debe ser mayor que 0.");

        RuleFor(x => x.Frecuencia)
            .MaximumLength(100).WithMessage("La frecuencia no puede superar los 100 caracteres.");

        RuleFor(x => x.Indicaciones)
            .NotEmpty().WithMessage("Las indicaciones son obligatorias.")
            .MaximumLength(500).WithMessage("Las indicaciones no pueden superar los 500 caracteres.");
    }
}

public class CrearAplicacionValidator : AbstractValidator<CrearAplicacionDto>
{
    public CrearAplicacionValidator()
    {
        RuleFor(x => x.FertilizanteId)
            .GreaterThan(0).WithMessage("El fertilizante es obligatorio.");

        RuleFor(x => x.Cantidad)
            .GreaterThan(0).WithMessage("La cantidad debe ser mayor que 0.");

        RuleFor(x => x.FechaAplicacion)
            .NotNull().WithMessage("La fecha de aplicación es obligatoria.")
            .LessThanOrEqualTo(_ => Fechas.Hoy)
            .WithMessage("La fecha de aplicación no puede ser futura.");

        RuleFor(x => x.Observaciones)
            .MaximumLength(250).WithMessage("Las observaciones no pueden superar los 250 caracteres.");
    }
}
