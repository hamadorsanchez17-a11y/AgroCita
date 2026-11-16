using FluentValidation;

namespace AgroCita.Application.Features.Auth.RegistroProductor;

public class RegistroProductorValidator : AbstractValidator<RegistroProductorDto>
{
    public RegistroProductorValidator()
    {
        RuleFor(x => x.CodigoInvitacion)
            .NotEmpty().WithMessage("El código de invitación es obligatorio.")
            .Length(12).WithMessage("El código de invitación debe tener 12 caracteres.");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(x => x.Apellido)
            .NotEmpty().WithMessage("El apellido es obligatorio.")
            .MaximumLength(100).WithMessage("El apellido no puede superar los 100 caracteres.");

        RuleFor(x => x.Dni)
            .NotEmpty().WithMessage("El DNI es obligatorio.")
            .Matches(@"^\d{13}$").WithMessage("El DNI debe tener 13 dígitos.");

        RuleFor(x => x.Telefono)
            .NotEmpty().WithMessage("El teléfono es obligatorio.")
            .Matches(@"^\d{8}$").WithMessage("El teléfono debe tener 8 dígitos.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
            .EmailAddress().WithMessage("El correo electrónico no tiene un formato válido.")
            .MaximumLength(120).WithMessage("El correo electrónico no puede superar los 120 caracteres.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria.")
            .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
            .Matches("[A-Z]").WithMessage("La contraseña debe incluir una mayúscula.")
            .Matches(@"\d").WithMessage("La contraseña debe incluir un número.");
    }
}
