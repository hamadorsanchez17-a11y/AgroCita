
using AgroCita.Application.Common.Exceptions;
using FluentValidation;

namespace AgroCita.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors
                .GroupBy(error => ErrorResponses.CamelCase(error.PropertyName))
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(error => error.ErrorMessage).ToArray()
                );

            await ErrorResponses.WriteAsync(
                context,
                StatusCodes.Status400BadRequest,
                "Datos inválidos",
                "Revise los campos marcados e intente de nuevo.",
                errors);
        }
        catch (NotFoundException ex)
        {
            await ErrorResponses.WriteAsync(
                context, StatusCodes.Status404NotFound, "No encontrado", ex.Message);
        }
        catch (ConflictException ex)
        {
            await ErrorResponses.WriteAsync(
                context, StatusCodes.Status409Conflict, "Conflicto", ex.Message);
        }
        catch (ForbiddenException ex)
        {
            await ErrorResponses.WriteAsync(
                context, StatusCodes.Status403Forbidden, "Acceso denegado", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrió un error inesperado.");

            await ErrorResponses.WriteAsync(
                context,
                StatusCodes.Status500InternalServerError,
                "Error interno del servidor",
                "Ocurrió un error inesperado. Intente de nuevo más tarde.");
        }
    }
}
