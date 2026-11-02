using AgroCita.Infrastructure;
using AgroCita.Infrastructure.Persistence.Context;
using AgroCita.API.Extensions;
using AgroCita.Application.Features.Auth.Login;

var builder = WebApplication.CreateBuilder(args);

// Registrar servicios de la API.
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<LoginHandler>();

//Registrar autenticación y autorización JWT
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

//Autenticar y autorizar solicitudes HTTP
app.UseAuthentication();
app.UseAuthorization();

// Registrar las rutas de los controladores.
app.MapControllers();

app.Run();
