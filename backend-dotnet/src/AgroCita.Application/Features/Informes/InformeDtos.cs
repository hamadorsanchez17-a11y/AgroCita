using AgroCita.Application.Common.Models;
using AgroCita.Application.Features.Calidad;
using AgroCita.Application.Features.Fertilizacion;
using AgroCita.Application.Features.Fincas;
using AgroCita.Application.Features.Tecnicos;
using AgroCita.Application.Features.Visitas;
using AgroCita.Domain.Entities;
using FluentValidation;

namespace AgroCita.Application.Features.Informes;

public record CrearInformeDto(string Observaciones, string Diagnostico, string Recomendaciones);

public record InformeDto(
    int Id,
    int VisitaId,
    DateTime Fecha,
    ReferenciaDto Tecnico,
    string Observaciones,
    string Diagnostico,
    string Recomendaciones,
    IReadOnlyList<InspeccionDto> Inspecciones);

public record InformeResumenDto(
    int Id,
    int VisitaId,
    DateTime Fecha,
    ReferenciaDto Finca,
    ReferenciaDto Tecnico);

public record HistorialDto(
    ReferenciaDto? Finca,
    ReferenciaDto? Cultivo,
    IReadOnlyList<VisitaDto> Visitas,
    IReadOnlyList<InspeccionDto> Inspecciones,
    IReadOnlyList<RecomendacionDto> Recomendaciones,
    IReadOnlyList<AplicacionDto> Aplicaciones);

public static class InformeMapeos
{
    public static readonly string[] RelacionesResumen =
        ["Visita.Solicitud.Finca", "Visita.Tecnico.Usuario"];

    public static InformeDto ADto(this Report informe, Visit visita, IEnumerable<Inspection> inspecciones) => new(
        informe.Id,
        informe.VisitaId,
        informe.FechaElaboracion,
        visita.Tecnico?.AReferencia() ?? new ReferenciaDto(visita.TecnicoId, string.Empty),
        informe.Observaciones,
        informe.Diagnostico,
        informe.Recomendaciones,
        inspecciones.Select(i => i.ADto()).ToList());

    public static InformeResumenDto AResumenDto(this Report informe) => new(
        informe.Id,
        informe.VisitaId,
        informe.FechaElaboracion,
        informe.Visita?.Solicitud?.Finca?.AReferencia() ?? new ReferenciaDto(0, string.Empty),
        informe.Visita?.Tecnico?.AReferencia() ?? new ReferenciaDto(0, string.Empty));
}

public class CrearInformeValidator : AbstractValidator<CrearInformeDto>
{
    public CrearInformeValidator()
    {
        RuleFor(x => x.Observaciones)
            .NotEmpty().WithMessage("Las observaciones son obligatorias.")
            .MaximumLength(4000).WithMessage("Las observaciones no pueden superar los 4000 caracteres.");

        RuleFor(x => x.Diagnostico)
            .NotEmpty().WithMessage("El diagnóstico es obligatorio.")
            .MaximumLength(4000).WithMessage("El diagnóstico no puede superar los 4000 caracteres.");

        RuleFor(x => x.Recomendaciones)
            .NotEmpty().WithMessage("Las recomendaciones son obligatorias.")
            .MaximumLength(4000).WithMessage("Las recomendaciones no pueden superar los 4000 caracteres.");
    }
}
