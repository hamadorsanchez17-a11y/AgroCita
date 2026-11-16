using AgroCita.Application.Common.Interfaces;
using AgroCita.Application.Common.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AgroCita.API.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class SinValidarSuscripcionAttribute : Attribute;

public class SuscripcionActivaFilter(
    ICurrentUserService usuarioActual,
    SuscripcionService suscripcion) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var omitir = context.ActionDescriptor.EndpointMetadata
            .OfType<SinValidarSuscripcionAttribute>()
            .Any();

        if (!omitir && usuarioActual.EmpresaId is not null)
        {
            await suscripcion.ExigirActivaAsync(context.HttpContext.RequestAborted);
        }

        await next();
    }
}
