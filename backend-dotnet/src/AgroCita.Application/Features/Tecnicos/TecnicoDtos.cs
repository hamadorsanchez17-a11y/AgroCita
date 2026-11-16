using AgroCita.Application.Common.Models;
using AgroCita.Domain.Entities;
using FluentValidation;

namespace AgroCita.Application.Features.Tecnicos;

public record CrearTecnicoDto(
    string Nombre,
    string Apellido,
    string Email,
    string Telefono,
    string? Especialidad,
    string PasswordTemporal);

public record ActualizarTecnicoDto(
    string Nombre,
    string Apellido,
    string Telefono,
    string? Especialidad);

public record CambiarEstadoUsuarioDto(bool? Activo);

public record TecnicoDto(
    int Id,
    string Nombre,
    string Apellido,
    string Email,
    string Telefono,
    string? Especialidad,
    bool Activo);

public static class TecnicoMapeos
{
    public static TecnicoDto ADto(this Technician tecnico) => new(
        tecnico.Id,
        tecnico.Usuario?.Nombre ?? string.Empty,
        tecnico.Usuario?.Apellido ?? string.Empty,
        tecnico.Usuario?.Email ?? string.Empty,
        tecnico.Telefono,
        tecnico.Especialidad,
        tecnico.Activo);

    public static ReferenciaDto AReferencia(this Technician tecnico) =>
        new(tecnico.Id, tecnico.Usuario?.NombreCompleto ?? string.Empty);
}

public class CrearTecnicoValidator : AbstractValidator<CrearTecnicoDto>
{
    public CrearTecnicoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(x => x.Apellido)
            .NotEmpty().WithMessage("El apellido es obligatorio.")
            .MaximumLength(100).WithMessage("El apellido no puede superar los 100 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
            .EmailAddress().WithMessage("El correo electrónico no tiene un formato válido.")
            .MaximumLength(120).WithMessage("El correo electrónico no puede superar los 120 caracteres.");

        RuleFor(x => x.Telefono)
            .NotEmpty().WithMessage("El teléfono es obligatorio.")
            .Matches(@"^\d{8}$").WithMessage("El teléfono debe tener 8 dígitos.");

        RuleFor(x => x.Especialidad)
            .MaximumLength(100).WithMessage("La especialidad no puede superar los 100 caracteres.");

        RuleFor(x => x.PasswordTemporal)
            .NotEmpty().WithMessage("La contraseña temporal es obligatoria.")
            .MinimumLength(8).WithMessage("La contraseña temporal debe tener al menos 8 caracteres.");
    }
}

public class ActualizarTecnicoValidator : AbstractValidator<ActualizarTecnicoDto>
{
    public ActualizarTecnicoValidator()
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

        RuleFor(x => x.Especialidad)
            .MaximumLength(100).WithMessage("La especialidad no puede superar los 100 caracteres.");
    }
}

public class CambiarEstadoUsuarioValidator : AbstractValidator<CambiarEstadoUsuarioDto>
{
    public CambiarEstadoUsuarioValidator()
    {
        RuleFor(x => x.Activo)
            .NotNull().WithMessage("Debe indicar si el usuario queda activo.");
    }
}
