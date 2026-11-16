using AgroCita.Application.Common.Models;
using AgroCita.Application.Features.Fincas;
using AgroCita.Domain.Entities;
using FluentValidation;

namespace AgroCita.Application.Features.Productores;

public record ActualizarProductorDto(
    string Nombre,
    string Apellido,
    string Telefono,
    string? Direccion);

public record ProductorDto
{
    public int Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string Apellido { get; init; } = string.Empty;
    public string Dni { get; init; } = string.Empty;
    public string Telefono { get; init; } = string.Empty;
    public string? Direccion { get; init; }
    public string Email { get; init; } = string.Empty;
}

public record ProductorDetalleDto : ProductorDto
{
    public IReadOnlyList<FincaDto> Fincas { get; init; } = [];
}

public record ProductorResumenDto(
    int Id,
    string NombreCompleto,
    string Dni,
    string Telefono,
    int CantidadFincas);

public static class ProductorMapeos
{
    private static T ADto<T>(Producer productor) where T : ProductorDto, new() => new()
    {
        Id = productor.Id,
        Nombre = productor.Usuario?.Nombre ?? string.Empty,
        Apellido = productor.Usuario?.Apellido ?? string.Empty,
        Dni = productor.Dni,
        Telefono = productor.Telefono,
        Direccion = productor.Direccion,
        Email = productor.Usuario?.Email ?? string.Empty
    };

    public static ProductorDto ADto(this Producer productor) => ADto<ProductorDto>(productor);

    public static ProductorDetalleDto ADetalleDto(this Producer productor) =>
        ADto<ProductorDetalleDto>(productor) with
        {
            Fincas = productor.Fincas.OrderBy(f => f.Nombre).Select(f => f.ADto()).ToList()
        };

    public static ProductorResumenDto AResumenDto(this Producer productor) => new(
        productor.Id,
        productor.Usuario?.NombreCompleto ?? string.Empty,
        productor.Dni,
        productor.Telefono,
        productor.Fincas.Count);

    public static ReferenciaDto AReferencia(this Producer productor) =>
        new(productor.Id, productor.Usuario?.NombreCompleto ?? string.Empty);
}

public class ActualizarProductorValidator : AbstractValidator<ActualizarProductorDto>
{
    public ActualizarProductorValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(x => x.Apellido)
            .NotEmpty().WithMessage("El apellido es obligatorio.")
            .MaximumLength(100).WithMessage("El apellido no puede superar los 100 caracteres.");

        RuleFor(x => x.Telefono)
            .NotEmpty().WithMessage("El teléfono es obligatorio.")
            .Matches(@"^\d{8}$").WithMessage("El teléfono debe tener 8 dígitos.");

        RuleFor(x => x.Direccion)
            .MaximumLength(250).WithMessage("La dirección no puede superar los 250 caracteres.");
    }
}
