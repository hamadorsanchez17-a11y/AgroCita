using AgroCita.Domain.Entities;
using FluentValidation;

namespace AgroCita.Application.Features.Admin;

public record PlanRequestDto(
    string Nombre,
    string? Descripcion,
    decimal PrecioMensual,
    int? MaxTecnicos,
    int? MaxProductores,
    bool? Activo);

public record PlanDto(
    int Id,
    string Nombre,
    string? Descripcion,
    decimal PrecioMensual,
    int? MaxTecnicos,
    int? MaxProductores,
    bool Activo);

public record AdminEmpresaDto(string Nombre, string? Apellido, string Email, string Password);

public record CrearEmpresaDto(
    string Nombre,
    string Rtn,
    string Email,
    string Telefono,
    string? Direccion,
    int PlanId,
    AdminEmpresaDto? Admin);

public record EmpresaResumenDto(
    int Id,
    string Nombre,
    string Rtn,
    string? Plan,
    SubscriptionStatus? EstadoSuscripcion,
    int? SuscripcionId);

public record AsignarPlanDto(int PlanId, DateOnly? FechaInicio, DateOnly? FechaFin);

public record CambiarEstadoSuscripcionDto(SubscriptionStatus? Estado);

public static class AdminMapeos
{
    public static PlanDto ADto(this Plan plan) => new(
        plan.Id,
        plan.Nombre,
        plan.Descripcion,
        plan.PrecioMensual,
        plan.MaxTecnicos,
        plan.MaxProductores,
        plan.Activo);

    public static EmpresaResumenDto AResumenDto(this Company empresa)
    {
        var vigente = empresa.SuscripcionVigente();

        return new EmpresaResumenDto(
            empresa.Id,
            empresa.Nombre,
            empresa.Rtn,
            vigente?.Plan?.Nombre,
            vigente?.Estado,
            vigente?.Id);
    }
}

public class PlanRequestValidator : AbstractValidator<PlanRequestDto>
{
    public PlanRequestValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del plan es obligatorio.")
            .MaximumLength(60).WithMessage("El nombre no puede superar los 60 caracteres.");

        RuleFor(x => x.Descripcion)
            .MaximumLength(250).WithMessage("La descripción no puede superar los 250 caracteres.");

        RuleFor(x => x.PrecioMensual)
            .GreaterThan(0).WithMessage("El precio mensual debe ser mayor que 0.");

        RuleFor(x => x.MaxTecnicos)
            .GreaterThan(0).WithMessage("El máximo de técnicos debe ser mayor que 0.");

        RuleFor(x => x.MaxProductores)
            .GreaterThan(0).WithMessage("El máximo de productores debe ser mayor que 0.");
    }
}

public class CrearEmpresaValidator : AbstractValidator<CrearEmpresaDto>
{
    public CrearEmpresaValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre de la organización es obligatorio.")
            .MaximumLength(150).WithMessage("El nombre no puede superar los 150 caracteres.");

        RuleFor(x => x.Rtn)
            .NotEmpty().WithMessage("El RTN es obligatorio.")
            .Matches(@"^\d{14}$").WithMessage("El RTN debe tener 14 dígitos.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
            .EmailAddress().WithMessage("El correo electrónico no tiene un formato válido.")
            .MaximumLength(120).WithMessage("El correo electrónico no puede superar los 120 caracteres.");

        RuleFor(x => x.Telefono)
            .NotEmpty().WithMessage("El teléfono es obligatorio.")
            .Matches(@"^\d{8}$").WithMessage("El teléfono debe tener 8 dígitos.");

        RuleFor(x => x.Direccion)
            .MaximumLength(250).WithMessage("La dirección no puede superar los 250 caracteres.");

        RuleFor(x => x.PlanId)
            .GreaterThan(0).WithMessage("El plan es obligatorio.");

        RuleFor(x => x.Admin)
            .NotNull().WithMessage("Los datos del administrador son obligatorios.");

        When(x => x.Admin is not null, () =>
        {
            RuleFor(x => x.Admin!.Nombre)
                .NotEmpty().WithMessage("El nombre del administrador es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

            RuleFor(x => x.Admin!.Apellido)
                .MaximumLength(100).WithMessage("El apellido no puede superar los 100 caracteres.");

            RuleFor(x => x.Admin!.Email)
                .NotEmpty().WithMessage("El correo del administrador es obligatorio.")
                .EmailAddress().WithMessage("El correo del administrador no tiene un formato válido.")
                .MaximumLength(120).WithMessage("El correo no puede superar los 120 caracteres.");

            RuleFor(x => x.Admin!.Password)
                .NotEmpty().WithMessage("La contraseña del administrador es obligatoria.")
                .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
                .Matches("[A-Z]").WithMessage("La contraseña debe incluir una mayúscula.")
                .Matches(@"\d").WithMessage("La contraseña debe incluir un número.");
        });
    }
}

public class AsignarPlanValidator : AbstractValidator<AsignarPlanDto>
{
    public AsignarPlanValidator()
    {
        RuleFor(x => x.PlanId)
            .GreaterThan(0).WithMessage("El plan es obligatorio.");

        RuleFor(x => x.FechaInicio)
            .NotNull().WithMessage("La fecha de inicio es obligatoria.");

        RuleFor(x => x.FechaFin)
            .Must((dto, fin) => fin is null || dto.FechaInicio is null || fin > dto.FechaInicio)
            .WithMessage("La fecha de fin debe ser posterior a la fecha de inicio.");
    }
}

public class CambiarEstadoSuscripcionValidator : AbstractValidator<CambiarEstadoSuscripcionDto>
{
    public CambiarEstadoSuscripcionValidator()
    {
        RuleFor(x => x.Estado)
            .NotNull().WithMessage("El estado es obligatorio.")
            .IsInEnum().WithMessage("El estado no es válido.");
    }
}
