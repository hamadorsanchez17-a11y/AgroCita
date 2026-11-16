using FluentValidation;
using FluentValidation.Results;

namespace AgroCita.Application.Common.Exceptions;

public class NotFoundException(string mensaje) : Exception(mensaje);

public class ConflictException(string mensaje) : Exception(mensaje);

public class ForbiddenException(string mensaje) : Exception(mensaje);

public static class Errores
{
    public static ValidationException Campo(string campo, string mensaje) =>
        new([new ValidationFailure(campo, mensaje)]);
}
