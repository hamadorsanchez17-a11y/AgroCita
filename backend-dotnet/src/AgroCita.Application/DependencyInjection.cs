using AgroCita.Application.Features.Auth.Login;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AgroCita.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddScoped<LoginHandler>();

        return services;
    }
}