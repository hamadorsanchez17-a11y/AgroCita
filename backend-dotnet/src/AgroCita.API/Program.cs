
using AgroCita.Infrastructure;
using AgroCita.Infrastructure.Persistence.Context;
using AgroCita.Application;
using AgroCita.API.Extensions;
using AgroCita.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Registrar servicios de la aplicación y la infraestructura.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Registrar autenticación y autorización JWT.
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

// Configurar el pipeline HTTP.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

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

app.UseHttpsRedirection();

// Middleware global de excepciones.
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
