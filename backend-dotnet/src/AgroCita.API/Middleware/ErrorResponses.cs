using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AgroCita.API.Middleware;

public record ErrorResponse(
    string Title,
    int Status,
    string Detail,
    IDictionary<string, string[]>? Errors = null);

public static class ErrorResponses
{
    public static Task WriteAsync(
        HttpContext context,
        int status,
        string title,
        string detail,
        IDictionary<string, string[]>? errors = null)
    {
        if (context.Response.HasStarted)
        {
            return Task.CompletedTask;
        }

        context.Response.StatusCode = status;

        return context.Response.WriteAsJsonAsync(
            new ErrorResponse(title, status, detail, errors));
    }

    public static IActionResult FromModelState(ModelStateDictionary modelState)
    {
        var errors = modelState
            .Where(entry => entry.Value is { Errors.Count: > 0 })
            .GroupBy(entry => CamelCase(entry.Key.StartsWith("$.") ? entry.Key[2..] : entry.Key))
            .ToDictionary(
                group => group.Key.Length == 0 || group.Key == "$" ? "body" : group.Key,
                group => new[] { "El valor enviado no tiene un formato válido." });

        return new BadRequestObjectResult(new ErrorResponse(
            "Datos inválidos",
            StatusCodes.Status400BadRequest,
            "La petición no tiene un formato válido.",
            errors));
    }

    public static string CamelCase(string propertyPath) =>
        string.Join('.', propertyPath
            .Split('.')
            .Select(part => part.Length == 0 ? part : char.ToLowerInvariant(part[0]) + part[1..]));
}
