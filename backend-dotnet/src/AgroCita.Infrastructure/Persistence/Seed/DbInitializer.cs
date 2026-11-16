using AgroCita.Application.Common.Interfaces;
using AgroCita.Domain.Constants;
using AgroCita.Domain.Entities;
using AgroCita.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AgroCita.Infrastructure.Persistence.Seed;

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider services, IConfiguration configuration)
    {
        using var scope = services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<AgroCitaDbContext>();
        var hash = scope.ServiceProvider.GetRequiredService<IPasswordHashService>();

        await db.Database.MigrateAsync();

        await SembrarPlanesAsync(db);
        await SembrarSuperAdminAsync(db, hash, configuration);

        if (configuration.GetValue<bool>("Seed:DemoData") &&
            !await db.Companies.IgnoreQueryFilters().AnyAsync())
        {
            await SembrarDemoAsync(db, hash);
        }
    }

    private static async Task SembrarPlanesAsync(AgroCitaDbContext db)
    {
        if (await db.Plans.AnyAsync())
        {
            return;
        }

        db.Plans.AddRange(
            new Plan
            {
                Nombre = "Básico",
                Descripcion = "Hasta 3 técnicos y 100 productores",
                PrecioMensual = 1200,
                MaxTecnicos = 3,
                MaxProductores = 100,
                Activo = true
            },
            new Plan
            {
                Nombre = "Profesional",
                Descripcion = "Hasta 10 técnicos y 500 productores",
                PrecioMensual = 2800,
                MaxTecnicos = 10,
                MaxProductores = 500,
                Activo = true
            },
            new Plan
            {
                Nombre = "Empresarial",
                Descripcion = "Técnicos y productores ilimitados",
                PrecioMensual = 5500,
                Activo = true
            });

        await db.SaveChangesAsync();
    }

    private static async Task SembrarSuperAdminAsync(
        AgroCitaDbContext db, IPasswordHashService hash, IConfiguration configuration)
    {
        var email = configuration["Seed:SuperAdminEmail"]?.Trim().ToLowerInvariant();
        var password = configuration["Seed:SuperAdminPassword"];

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return;
        }

        if (await db.Users.IgnoreQueryFilters().AnyAsync(u => u.RolId == Roles.SuperAdminId))
        {
            return;
        }

        db.Users.Add(Usuario(hash, null, Roles.SuperAdminId, "Administrador", "AgroCita", email, password));
        await db.SaveChangesAsync();
    }

    private static async Task SembrarDemoAsync(AgroCitaDbContext db, IPasswordHashService hash)
    {
        var planes = await db.Plans.ToListAsync();
        var hoy = DateOnly.FromDateTime(DateTime.UtcNow);

        var agroConsulta = Empresa(
            "AgroConsulta", "08019999123456", "contacto@agroconsulta.hn", "22334455",
            "Col. Palmira, Tegucigalpa", "AGROCONSU001",
            planes.First(p => p.Nombre == "Profesional"), hoy);

        var agroServicios = Empresa(
            "AgroServicios", "05019999654321", "contacto@agroservicios.hn", "25506677",
            "Barrio Guamilito, San Pedro Sula", "AGROSERVI002",
            planes.First(p => p.Nombre == "Básico"), hoy);

        db.Companies.AddRange(agroConsulta, agroServicios);
        await db.SaveChangesAsync();

        var empresaId = agroConsulta.Id;

        db.Users.AddRange(
            Usuario(hash, empresaId, Roles.AdminEmpresaId, "Ana", "Reyes", "admin@agroconsulta.hn", "Admin123*"),
            Usuario(hash, agroServicios.Id, Roles.AdminEmpresaId, "Luis", "Paz", "admin@agroservicios.hn", "Admin123*"));

        var tecnico = new Technician
        {
            EmpresaId = empresaId,
            Especialidad = "Caficultura",
            Telefono = "98765432",
            Activo = true,
            Usuario = Usuario(hash, empresaId, Roles.TecnicoId, "Carlos", "Mejía", "tecnico@agroconsulta.hn", "Tecnico123*")
        };

        var cafe = new CropType { EmpresaId = empresaId, Nombre = "Café" };
        var maiz = new CropType { EmpresaId = empresaId, Nombre = "Maíz" };
        var frijol = new CropType { EmpresaId = empresaId, Nombre = "Frijol" };

        var cultivoCafe = new Crop
        {
            EmpresaId = empresaId,
            TipoCultivo = cafe,
            Nombre = "Café Lempira lote 1",
            Variedad = "Lempira",
            AreaSembrada = 8,
            FechaSiembra = hoy.AddYears(-3),
            Activo = true
        };

        var cultivoMaiz = new Crop
        {
            EmpresaId = empresaId,
            TipoCultivo = maiz,
            Nombre = "Maíz de primera",
            Variedad = "Dekalb 390",
            AreaSembrada = 3.5m,
            FechaSiembra = hoy.AddMonths(-2),
            Activo = true
        };

        var finca = new Farm
        {
            EmpresaId = empresaId,
            Nombre = "Finca El Paraíso",
            Departamento = "Francisco Morazán",
            Municipio = "Valle de Ángeles",
            Ubicacion = "Aldea Las Cañadas, km 5 carretera a San Juancito",
            AreaManzanas = 12.5m,
            Descripcion = "Finca cafetalera con parcelas de granos básicos",
            Cultivos = [cultivoCafe, cultivoMaiz]
        };

        var productor = new Producer
        {
            EmpresaId = empresaId,
            Dni = "0801199012345",
            Telefono = "99887766",
            Direccion = "Barrio El Centro, Valle de Ángeles",
            Usuario = Usuario(hash, empresaId, Roles.ProductorId, "María", "López", "productor@agroconsulta.hn", "Productor123*"),
            Fincas = [finca]
        };

        db.Technicians.Add(tecnico);
        db.CropTypes.Add(frijol);
        db.Producers.Add(productor);

        db.EvaluationParameters.AddRange(
            new EvaluationParameter { EmpresaId = empresaId, TipoCultivo = cafe, Nombre = "Humedad del grano", Unidad = "%", ValorMinimo = 10, ValorMaximo = 12.5m },
            new EvaluationParameter { EmpresaId = empresaId, TipoCultivo = cafe, Nombre = "Incidencia de roya", Unidad = "%", ValorMinimo = 0, ValorMaximo = 100 },
            new EvaluationParameter { EmpresaId = empresaId, TipoCultivo = maiz, Nombre = "Altura de planta", Unidad = "cm" },
            new EvaluationParameter { EmpresaId = empresaId, TipoCultivo = frijol, Nombre = "Vainas por planta", Unidad = "unidades" });

        db.QualityCategories.AddRange(
            new[] { "Grado A", "Grado B", "Orgánico", "Exportación", "Primera selección", "Segunda selección" }
                .Select(nombre => new QualityCategory { EmpresaId = empresaId, Nombre = nombre }));

        db.Fertilizers.AddRange(
            new Fertilizer { EmpresaId = empresaId, Nombre = "Urea 46%", Tipo = "Nitrogenado", UnidadMedida = "lb", Composicion = "46-0-0" },
            new Fertilizer { EmpresaId = empresaId, Nombre = "Fórmula 18-46-0", Tipo = "Fosfatado", UnidadMedida = "lb", Composicion = "18-46-0" },
            new Fertilizer { EmpresaId = empresaId, Nombre = "Compost orgánico", Tipo = "Orgánico", UnidadMedida = "qq" });

        db.AssistanceRequests.Add(new AssistanceRequest
        {
            EmpresaId = empresaId,
            Finca = finca,
            Motivo = "Hojas amarillas y manchas en el cafetal del lote 1.",
            FechaDeseada = hoy.AddDays(7),
            Estado = AssistanceRequestStatus.Pendiente,
            FechaCreacion = DateTime.UtcNow,
            Cultivos = [new AssistanceRequestCrop { EmpresaId = empresaId, Cultivo = cultivoCafe }]
        });

        await db.SaveChangesAsync();
    }

    private static Company Empresa(
        string nombre, string rtn, string email, string telefono,
        string direccion, string codigo, Plan plan, DateOnly hoy) => new()
    {
        Nombre = nombre,
        Rtn = rtn,
        Email = email,
        Telefono = telefono,
        Direccion = direccion,
        CodigoInvitacion = codigo,
        Activa = true,
        FechaRegistro = DateTime.UtcNow,
        Suscripciones =
        [
            new Subscription
            {
                Plan = plan,
                FechaInicio = hoy,
                Estado = SubscriptionStatus.Activa
            }
        ]
    };

    private static User Usuario(
        IPasswordHashService hash, int? empresaId, int rolId,
        string nombre, string apellido, string email, string password) => new()
    {
        EmpresaId = empresaId,
        RolId = rolId,
        Nombre = nombre,
        Apellido = apellido,
        Email = email,
        PasswordHash = hash.HashPassword(password),
        Activo = true,
        FechaCreacion = DateTime.UtcNow
    };
}
