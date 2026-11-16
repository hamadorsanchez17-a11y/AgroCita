using AgroCita.Application.Common.Models;
using AgroCita.Application.Features.Fincas;
using AgroCita.Application.Features.Productores;
using AgroCita.Application.Features.Tecnicos;
using AgroCita.Domain.Entities;
using FluentValidation;

namespace AgroCita.Application.Features.Visitas;

public record CrearVisitaDto(int SolicitudId, int TecnicoId, DateTime? FechaProgramada);

public record CambiarEstadoVisitaDto(VisitStatus? Estado, string? Motivo);

public record ReprogramarVisitaDto(DateTime? NuevaFecha, string Motivo);

public record VisitaDto
{
    public int Id { get; init; }
    public int SolicitudId { get; init; }
    public ReferenciaDto Finca { get; init; } = new(0, string.Empty);
    public ReferenciaDto Productor { get; init; } = new(0, string.Empty);
    public ReferenciaDto Tecnico { get; init; } = new(0, string.Empty);
    public DateTime FechaProgramada { get; init; }
    public VisitStatus Estado { get; init; }
    public string? MotivoCancelacion { get; init; }
}

public record UbicacionDto(string Departamento, string Municipio, string Direccion);

public record VisitaDetalleDto : VisitaDto
{
    public UbicacionDto Ubicacion { get; init; } = new(string.Empty, string.Empty, string.Empty);
    public IReadOnlyList<ReferenciaDto> Cultivos { get; init; } = [];
    public string Motivo { get; init; } = string.Empty;
    public bool TieneInforme { get; init; }
}

public record VisitaAgendaDto(
    int Id,
    DateTime FechaProgramada,
    ReferenciaDto Tecnico,
    ReferenciaDto Finca,
    VisitStatus Estado);

public record ReprogramacionDto(
    DateTime FechaAnterior,
    DateTime FechaNueva,
    string Motivo,
    DateTime FechaCambio,
    string RealizadoPor);

public static class VisitaMapeos
{
    public static readonly string[] Relaciones =
        ["Solicitud.Finca.Productor.Usuario", "Tecnico.Usuario"];

    public static readonly string[] RelacionesDetalle =
        [.. Relaciones, "Solicitud.Cultivos.Cultivo", "Informe"];

    private static T ADto<T>(Visit visita) where T : VisitaDto, new()
    {
        var finca = visita.Solicitud?.Finca;

        return new T
        {
            Id = visita.Id,
            SolicitudId = visita.SolicitudId,
            Finca = finca?.AReferencia() ?? new ReferenciaDto(0, string.Empty),
            Productor = finca?.Productor?.AReferencia() ?? new ReferenciaDto(0, string.Empty),
            Tecnico = visita.Tecnico?.AReferencia() ?? new ReferenciaDto(visita.TecnicoId, string.Empty),
            FechaProgramada = visita.FechaProgramada,
            Estado = visita.Estado,
            MotivoCancelacion = visita.MotivoCancelacion
        };
    }

    public static VisitaDto ADto(this Visit visita) => ADto<VisitaDto>(visita);

    public static VisitaDetalleDto ADetalleDto(this Visit visita)
    {
        var solicitud = visita.Solicitud!;
        var finca = solicitud.Finca!;

        return ADto<VisitaDetalleDto>(visita) with
        {
            Ubicacion = new UbicacionDto(finca.Departamento, finca.Municipio, finca.Ubicacion),
            Cultivos = solicitud.Cultivos
                .Where(c => c.Cultivo is not null)
                .Select(c => c.Cultivo!.AReferencia())
                .ToList(),
            Motivo = solicitud.Motivo,
            TieneInforme = visita.Informe is not null
        };
    }

    public static VisitaAgendaDto AAgendaDto(this Visit visita)
    {
        var dto = visita.ADto();

        return new VisitaAgendaDto(dto.Id, dto.FechaProgramada, dto.Tecnico, dto.Finca, dto.Estado);
    }

    public static ReprogramacionDto ADto(this VisitReschedule cambio) => new(
        cambio.FechaAnterior,
        cambio.FechaNueva,
        cambio.Motivo,
        cambio.FechaRegistro,
        cambio.Usuario?.NombreCompleto ?? string.Empty);
}

public class CrearVisitaValidator : AbstractValidator<CrearVisitaDto>
{
    public CrearVisitaValidator()
    {
        RuleFor(x => x.SolicitudId)
            .GreaterThan(0).WithMessage("La solicitud es obligatoria.");

        RuleFor(x => x.TecnicoId)
            .GreaterThan(0).WithMessage("El técnico es obligatorio.");

        RuleFor(x => x.FechaProgramada)
            .NotNull().WithMessage("La fecha programada es obligatoria.")
            .Must(f => f is null || f.Value.AUtc() > Fechas.Ahora)
            .WithMessage("La fecha programada debe ser futura.");
    }
}

public class CambiarEstadoVisitaValidator : AbstractValidator<CambiarEstadoVisitaDto>
{
    public CambiarEstadoVisitaValidator()
    {
        RuleFor(x => x.Estado)
            .NotNull().WithMessage("El estado es obligatorio.")
            .IsInEnum().WithMessage("El estado no es válido.");

        RuleFor(x => x.Motivo)
            .NotEmpty().WithMessage("El motivo es obligatorio al cancelar una visita.")
            .When(x => x.Estado == VisitStatus.Cancelada);

        RuleFor(x => x.Motivo)
            .MaximumLength(250).WithMessage("El motivo no puede superar los 250 caracteres.");
    }
}

public class ReprogramarVisitaValidator : AbstractValidator<ReprogramarVisitaDto>
{
    public ReprogramarVisitaValidator()
    {
        RuleFor(x => x.NuevaFecha)
            .NotNull().WithMessage("La nueva fecha es obligatoria.")
            .Must(f => f is null || f.Value.AUtc() > Fechas.Ahora)
            .WithMessage("La nueva fecha debe ser futura.");

        RuleFor(x => x.Motivo)
            .NotEmpty().WithMessage("El motivo de la reprogramación es obligatorio.")
            .MaximumLength(250).WithMessage("El motivo no puede superar los 250 caracteres.");
    }
}
