
using AgroCita.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AgroCita.Application.Common.Interfaces;
using AgroCita.Infrastructure.Services.Security;
using AgroCita.Infrastructure.Persistence.Repositories;
using AgroCita.Application.Features.Auth.Login;

namespace AgroCita.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration
            .GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "No se encontró la cadena de conexión de PostgreSQL.");
        }

        services.AddDbContext<AgroCitaDbContext>(options =>
            options.UseNpgsql(connectionString));

      services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
services.AddScoped<IPasswordHashService, PasswordHashService>();
services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();

services.AddHttpContextAccessor();
services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
