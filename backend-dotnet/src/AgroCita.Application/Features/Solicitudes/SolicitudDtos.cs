using AgroCita.Application.Common.Models;
using AgroCita.Application.Features.Fincas;
using AgroCita.Application.Features.Productores;
using AgroCita.Application.Features.Visitas;
using AgroCita.Domain.Entities;
using FluentValidation;

namespace AgroCita.Application.Features.Solicitudes;

public record CrearSolicitudDto(
    int FincaId,
    List<int>? CultivoIds,
    DateOnly? FechaDeseada,
    string Motivo);

public record CambiarEstadoSolicitudDto(AssistanceRequestStatus? Estado, string? Comentario);

public record SolicitudDto
{
    public int Id { get; init; }
    public ReferenciaDto Finca { get; init; } = new(0, string.Empty);
    public ReferenciaDto Productor { get; init; } = new(0, string.Empty);
    public IReadOnlyList<ReferenciaDto> Cultivos { get; init; } = [];
    public DateOnly FechaDeseada { get; init; }
    public string Motivo { get; init; } = string.Empty;
    public AssistanceRequestStatus Estado { get; init; }
    public string? Comentario { get; init; }
    public DateTime FechaCreacion { get; init; }
}

public record SolicitudDetalleDto : SolicitudDto
{
    public IReadOnlyList<VisitaDto> Visitas { get; init; } = [];
}

public static class SolicitudMapeos
{
    public static readonly string[] Relaciones =
        ["Finca.Productor.Usuario", "Cultivos.Cultivo"];

    public static readonly string[] RelacionesDetalle =
        [.. Relaciones, "Visitas.Tecnico.Usuario"];

    private static T ADto<T>(AssistanceRequest solicitud) where T : SolicitudDto, new() => new()
    {
        Id = solicitud.Id,
        Finca = solicitud.Finca?.AReferencia() ?? new ReferenciaDto(solicitud.FincaId, string.Empty),
        Productor = solicitud.Finca?.Productor?.AReferencia() ?? new ReferenciaDto(0, string.Empty),
        Cultivos = solicitud.Cultivos
            .Where(c => c.Cultivo is not null)
            .Select(c => c.Cultivo!.AReferencia())
            .ToList(),
        FechaDeseada = solicitud.FechaDeseada,
        Motivo = solicitud.Motivo,
        Estado = solicitud.Estado,
        Comentario = solicitud.ComentarioEstado,
        FechaCreacion = solicitud.FechaCreacion
    };

    public static SolicitudDto ADto(this AssistanceRequest solicitud) =>
        ADto<SolicitudDto>(solicitud);

    public static SolicitudDetalleDto ADetalleDto(this AssistanceRequest solicitud) =>
        ADto<SolicitudDetalleDto>(solicitud) with
        {
            Visitas = solicitud.Visitas
                .OrderBy(v => v.FechaProgramada)
                .Select(v => v.ADto())
                .ToList()
        };
}

public class CrearSolicitudValidator : AbstractValidator<CrearSolicitudDto>
{
    public CrearSolicitudValidator()
    {
        RuleFor(x => x.FincaId)
            .GreaterThan(0).WithMessage("La finca es obligatoria.");

        RuleFor(x => x.CultivoIds)
            .NotEmpty().WithMessage("Debe seleccionar al menos un cultivo.");

        RuleFor(x => x.FechaDeseada)
            .NotNull().WithMessage("La fecha deseada es obligatoria.")
            .GreaterThanOrEqualTo(_ => Fechas.Hoy)
            .WithMessage("La fecha deseada no puede ser pasada.");

        RuleFor(x => x.Motivo)
            .NotEmpty().WithMessage("El motivo es obligatorio.")
            .MaximumLength(500).WithMessage("El motivo no puede superar los 500 caracteres.");
    }
}

public class CambiarEstadoSolicitudValidator : AbstractValidator<CambiarEstadoSolicitudDto>
{
    public CambiarEstadoSolicitudValidator()
    {
        RuleFor(x => x.Estado)
            .NotNull().WithMessage("El estado es obligatorio.")
            .IsInEnum().WithMessage("El estado no es válido.");

        RuleFor(x => x.Comentario)
            .NotEmpty().WithMessage("El comentario es obligatorio al rechazar una solicitud.")
            .When(x => x.Estado == AssistanceRequestStatus.Rechazada);

        RuleFor(x => x.Comentario)
            .MaximumLength(500).WithMessage("El comentario no puede superar los 500 caracteres.");
    }
}
