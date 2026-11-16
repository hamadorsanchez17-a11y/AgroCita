
using System.Text.Json.Serialization;
using AgroCita.Infrastructure;
using AgroCita.Infrastructure.Persistence.Context;
using AgroCita.Infrastructure.Persistence.Seed;
using AgroCita.Application;
using AgroCita.API.Extensions;
using AgroCita.API.Filters;
using AgroCita.API.Middleware;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Registrar servicios.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services
    .AddControllers(options =>
    {
        options.Filters.Add<SuscripcionActivaFilter>();
        options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    })
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
    .ConfigureApiBehaviorOptions(options =>
        options.InvalidModelStateResponseFactory = context =>
            ErrorResponses.FromModelState(context.ModelState));

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? [];

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy => policy
        .WithOrigins(allowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithExposedHeaders("Location")));

// Configuración de OpenAPI y autenticación Bearer.
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new Microsoft.OpenApi.OpenApiComponents();

        document.Components.SecuritySchemes ??=
            new Dictionary<string, Microsoft.OpenApi.IOpenApiSecurityScheme>();

        document.Components.SecuritySchemes["Bearer"] =
            new Microsoft.OpenApi.OpenApiSecurityScheme
            {
                Type = Microsoft.OpenApi.SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Introduce tu token JWT."
            };

        return Task.CompletedTask;
    });

    options.AddOperationTransformer((operation, context, cancellationToken) =>
    {
        var requiresAuthorization =
            context.Description.ActionDescriptor.EndpointMetadata
                .OfType<Microsoft.AspNetCore.Authorization.IAuthorizeData>()
                .Any();

        if (requiresAuthorization)
        {
            operation.Security ??=
                new List<Microsoft.OpenApi.OpenApiSecurityRequirement>();

            operation.Security.Add(
                new Microsoft.OpenApi.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.OpenApiSecuritySchemeReference(
                            "Bearer", context.Document),
                        new List<string>()
                    }
                });
        }

        return Task.CompletedTask;
    });
});

// Autenticación JWT.
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

await DbInitializer.InitializeAsync(app.Services, app.Configuration);

// Página principal.
app.MapGet("/", () =>
{
    return Results.Ok(new
    {
        mensaje = "Bienvenido a AgroCita API",
        estado = "Funcionando",
        documentacion = "/scalar",
        diagnostico = "/api/diagnostics/database"
    });
});

// Configuración de desarrollo.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference();

    // Diagnóstico de PostgreSQL.
    app.MapGet("/api/diagnostics/database", async (AgroCitaDbContext db) =>
    {
        var canConnect = await db.Database.CanConnectAsync();

        if (canConnect)
        {
            return Results.Ok(new
            {
                status = "Conectado",
                database = "PostgreSQL"
            });
        }

        return Results.Problem(
            "No fue posible conectar con PostgreSQL.",
            statusCode: StatusCodes.Status503ServiceUnavailable
        );
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
