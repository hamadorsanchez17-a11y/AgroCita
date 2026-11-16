using AgroCita.Application.Common.Models;
using FluentValidation;

namespace AgroCita.Application.Features.Fincas;

public class FincaRequestValidator : AbstractValidator<FincaRequestDto>
{
    public FincaRequestValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre de la finca es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(x => x.Departamento)
            .NotEmpty().WithMessage("El departamento es obligatorio.")
            .MaximumLength(60).WithMessage("El departamento no puede superar los 60 caracteres.");

        RuleFor(x => x.Municipio)
            .NotEmpty().WithMessage("El municipio es obligatorio.")
            .MaximumLength(60).WithMessage("El municipio no puede superar los 60 caracteres.");

        RuleFor(x => x.Direccion)
            .NotEmpty().WithMessage("La dirección es obligatoria.")
            .MaximumLength(250).WithMessage("La dirección no puede superar los 250 caracteres.");

        RuleFor(x => x.AreaManzanas)
            .GreaterThan(0).WithMessage("El área debe ser mayor que 0.");

        RuleFor(x => x.Descripcion)
            .MaximumLength(500).WithMessage("La descripción no puede superar los 500 caracteres.");
    }
}

public class CultivoRequestValidator : AbstractValidator<CultivoRequestDto>
{
    public CultivoRequestValidator()
    {
        RuleFor(x => x.TipoCultivoId)
            .GreaterThan(0).WithMessage("El tipo de cultivo es obligatorio.");

        RuleFor(x => x.Nombre)
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(x => x.Variedad)
            .MaximumLength(60).WithMessage("La variedad no puede superar los 60 caracteres.");

        RuleFor(x => x.AreaManzanas)
            .GreaterThan(0).WithMessage("El área debe ser mayor que 0.");

        RuleFor(x => x.FechaSiembra)
            .LessThanOrEqualTo(_ => Fechas.Hoy)
            .WithMessage("La fecha de siembra no puede ser futura.");
    }
}

public class TipoCultivoRequestValidator : AbstractValidator<TipoCultivoRequestDto>
{
    public TipoCultivoRequestValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del tipo de cultivo es obligatorio.")
            .MaximumLength(60).WithMessage("El nombre no puede superar los 60 caracteres.");
    }
}
