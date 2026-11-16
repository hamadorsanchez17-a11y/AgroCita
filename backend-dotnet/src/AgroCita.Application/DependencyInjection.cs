using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AgroCita.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddValidatorsFromAssembly(assembly);

        var casosDeUso = assembly.GetTypes().Where(t =>
            t is { IsClass: true, IsAbstract: false, IsNested: false } &&
            (t.Name.EndsWith("Handler") ||
             t.Namespace == "AgroCita.Application.Common.Services"));

        foreach (var tipo in casosDeUso)
        {
            services.AddScoped(tipo);
        }

        return services;
    }
}
