
using AgroCita.Application.Common.Interfaces;
using AgroCita.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AgroCita.Infrastructure.Persistence.Context;

public class AgroCitaDbContext : DbContext
{
    private readonly ICurrentUserService _currentUserService;

    public AgroCitaDbContext(
        DbContextOptions<AgroCitaDbContext> options,
        ICurrentUserService currentUserService)
        : base(options)
    {
        _currentUserService = currentUserService;
    }

    public int? EmpresaId => _currentUserService.EmpresaId;


public override int SaveChanges()
{
    AplicarSeguridadMultitenant();
    return base.SaveChanges();
}

public override int SaveChanges(bool acceptAllChangesOnSuccess)
{
    AplicarSeguridadMultitenant();
    return base.SaveChanges(acceptAllChangesOnSuccess);
}

public override Task<int> SaveChangesAsync(
    CancellationToken cancellationToken = default)
{
    AplicarSeguridadMultitenant();
    return base.SaveChangesAsync(cancellationToken);
}

public override Task<int> SaveChangesAsync(
    bool acceptAllChangesOnSuccess,
    CancellationToken cancellationToken = default)
{
    AplicarSeguridadMultitenant();
    return base.SaveChangesAsync(
        acceptAllChangesOnSuccess,
        cancellationToken);
}

private void AplicarSeguridadMultitenant()
{
    if (_currentUserService.EsSuperAdmin)
        return;

    if (EmpresaId is not int empresaId)
        throw new InvalidOperationException(
            "No se pudo identificar la empresa del usuario autenticado.");

    var entradas = ChangeTracker.Entries()
        .Where(e =>
            e.State == EntityState.Added ||
            e.State == EntityState.Modified ||
            e.State == EntityState.Deleted);

    foreach (var entrada in entradas)
    {
        // Proteger la entidad Empresa usando su Id.
        if (entrada.Entity is Company)
        {
            if (entrada.State == EntityState.Added)
            {
                throw new InvalidOperationException(
                    "No tienes permiso para registrar empresas.");
            }

            var idOriginal = Convert.ToInt32(
                entrada.Property("Id").OriginalValue);

            var idActual = Convert.ToInt32(
                entrada.Property("Id").CurrentValue);

            if (idOriginal != empresaId || idActual != empresaId)
            {
                throw new InvalidOperationException(
                    "No tienes permiso para modificar datos de otra empresa.");
            }

            continue;
        }

        // Los catálogos globales no pueden ser modificados
        // por usuarios pertenecientes a una empresa.
        if (entrada.Entity is Rol ||
            entrada.Entity is SubscriptionPlan)
        {
            throw new InvalidOperationException(
                "No tienes permiso para modificar catálogos globales.");
        }

        var propiedad = entrada.Metadata.FindProperty("EmpresaId");

        // Ignorar entidades que no pertenecen a una empresa.
        if (propiedad is null)
            continue;

        var empresa = entrada.Property("EmpresaId");

        if (entrada.State == EntityState.Added)
        {
            // Asignar automáticamente la empresa autenticada.
            empresa.CurrentValue = empresaId;
        }
        else
        {
            if (empresa.OriginalValue is not int empresaOriginal ||
                empresa.CurrentValue is not int empresaActual ||
                empresaOriginal != empresaId ||
                empresaActual != empresaId)
            {
                throw new InvalidOperationException(
                    "No tienes permiso para modificar datos de otra empresa.");
            }
        }
    }
}


    public DbSet<User> Users => Set<User>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Producer> Producers => Set<Producer>();
    public DbSet<Technician> Technicians => Set<Technician>();
    public DbSet<Farm> Farms => Set<Farm>();
    public DbSet<Crop> Crops => Set<Crop>();
    public DbSet<AssistanceRequest> AssistanceRequests => Set<AssistanceRequest>();
    public DbSet<AssistanceRequestCrop> AssistanceRequestCrops => Set<AssistanceRequestCrop>();
    public DbSet<Visit> Visits => Set<Visit>();


public DbSet<Rol> Roles => Set<Rol>();
public DbSet<TipoCultivo> TiposCultivo => Set<TipoCultivo>();

public DbSet<Inspection> Inspections => Set<Inspection>();
public DbSet<QualityCategory> QualityCategories => Set<QualityCategory>();
public DbSet<EvaluationParameter> EvaluationParameters => Set<EvaluationParameter>();
public DbSet<ParameterResult> ParameterResults => Set<ParameterResult>();

public DbSet<Fertilizer> Fertilizers => Set<Fertilizer>();
public DbSet<FertilizerRecommendation> FertilizerRecommendations => Set<FertilizerRecommendation>();
public DbSet<FertilizerApplication> FertilizerApplications => Set<FertilizerApplication>();

public DbSet<ReschedulingVisit> ReschedulingVisits => Set<ReschedulingVisit>();

public DbSet<SubscriptionPlan> SubscriptionPlans => Set<SubscriptionPlan>();
public DbSet<Subscription> Subscriptions => Set<Subscription>();

public DbSet<Report> Reports => Set<Report>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // USUARIOS
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("usuarios");

            entity.HasKey(u => u.Id);

            entity.Property(u => u.EmpresaId);

            entity.Property(u => u.Nombre)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(u => u.Apellido)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(u => u.PasswordHash)
                .IsRequired();

            entity.Property(u => u.RolId)
                .IsRequired();

            entity.HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.RolId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(u => u.Empresa)
                .WithMany(e => e.Usuarios)
                .HasForeignKey(u => u.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(u => u.Activo)
                .IsRequired();

            entity.HasIndex(u => u.Email)
                .IsUnique();
        });

        // CLAVE COMPUESTA DE LA RELACIÓN SOLICITUD-CULTIVO
        modelBuilder.Entity<AssistanceRequestCrop>()
            .HasKey(x => new { x.SolicitudId, x.CultivoId });


        // ROLES
        modelBuilder.Entity<Rol>(entity =>
        {
            entity.ToTable("roles");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Nombre)
                .IsRequired()
                .HasMaxLength(50);
        });

        // EMPRESAS
        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("empresas");
            entity.HasKey(x => x.Id);
        });

        // PRODUCTORES
        modelBuilder.Entity<Producer>(entity =>
        {
            entity.HasOne(x => x.Empresa)
                .WithMany(x => x.Productores)
                .HasForeignKey(x => x.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Usuario)
                .WithOne(x => x.Productor)
                .HasForeignKey<Producer>(x => x.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // TÉCNICOS
        modelBuilder.Entity<Technician>(entity =>
        {
            entity.HasOne(x => x.Empresa)
                .WithMany(x => x.Tecnicos)
                .HasForeignKey(x => x.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Usuario)
                .WithOne(x => x.Tecnico)
                .HasForeignKey<Technician>(x => x.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        });


        // FINCAS
        modelBuilder.Entity<Farm>(entity =>
        {
            entity.HasOne(x => x.Empresa)
                .WithMany(x => x.Fincas)
                .HasForeignKey(x => x.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Productor)
                .WithMany(x => x.Fincas)
                .HasForeignKey(x => new { x.ProductorId, x.EmpresaId })
                .HasPrincipalKey(x => new { x.Id, x.EmpresaId })
                .OnDelete(DeleteBehavior.Restrict);
        });


        // TIPOS DE CULTIVO
        modelBuilder.Entity<TipoCultivo>(entity =>
        {
            entity.HasOne(x => x.Empresa)
                .WithMany()
                .HasForeignKey(x => x.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);
        });


        // CULTIVOS
        modelBuilder.Entity<Crop>(entity =>
        {
            entity.HasOne(x => x.Empresa)
                .WithMany(x => x.Cultivos)
                .HasForeignKey(x => x.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Finca)
                .WithMany(x => x.Cultivos)
                .HasForeignKey(x => new { x.FincaId, x.EmpresaId })
                .HasPrincipalKey(x => new { x.Id, x.EmpresaId })
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.TipoCultivo)
                .WithMany(x => x.Cultivos)
                .HasForeignKey(x => new { x.TipoCultivoId, x.EmpresaId })
                .HasPrincipalKey(x => new { x.Id, x.EmpresaId })
                .OnDelete(DeleteBehavior.Restrict);
        });


        // SOLICITUDES

        modelBuilder.Entity<AssistanceRequest>(entity =>
        {
            entity.HasOne(x => x.Empresa)
                .WithMany()
                .HasForeignKey(x => x.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Finca)
                .WithMany(x => x.Solicitudes)
                .HasForeignKey(x => new { x.FincaId, x.EmpresaId })
                .HasPrincipalKey(x => new { x.Id, x.EmpresaId })
                .OnDelete(DeleteBehavior.Restrict);
        });


        // SOLICITUD - CULTIVO

modelBuilder.Entity<AssistanceRequestCrop>(entity =>
{
    entity.HasOne(x => x.Solicitud)
        .WithMany(x => x.Cultivos)
        .HasForeignKey(x => new { x.SolicitudId, x.EmpresaId })
        .HasPrincipalKey(x => new { x.Id, x.EmpresaId })
        .OnDelete(DeleteBehavior.Restrict);

    entity.HasOne(x => x.Cultivo)
        .WithMany(x => x.Solicitudes)
        .HasForeignKey(x => new { x.CultivoId, x.EmpresaId })
        .HasPrincipalKey(x => new { x.Id, x.EmpresaId })
        .OnDelete(DeleteBehavior.Restrict);

    entity.HasOne(x => x.Empresa)
        .WithMany()
        .HasForeignKey(x => x.EmpresaId)
        .OnDelete(DeleteBehavior.Restrict);
});


        // VISITAS

        modelBuilder.Entity<Visit>(entity =>
        {
            entity.HasOne(x => x.Empresa)
                .WithMany()
                .HasForeignKey(x => x.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Solicitud)
                .WithMany(x => x.Visitas)
                .HasForeignKey(x => new { x.SolicitudId, x.EmpresaId })
                .HasPrincipalKey(x => new { x.Id, x.EmpresaId })
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Tecnico)
                .WithMany(x => x.Visitas)
                .HasForeignKey(x => new { x.TecnicoId, x.EmpresaId })
                .HasPrincipalKey(x => new { x.Id, x.EmpresaId })
                .OnDelete(DeleteBehavior.Restrict);
        });


        // REPROGRAMACIONES

modelBuilder.Entity<ReschedulingVisit>(entity =>
{
    entity.HasOne(x => x.Empresa)
        .WithMany()
        .HasForeignKey(x => x.EmpresaId)
        .OnDelete(DeleteBehavior.Restrict);

    entity.HasOne(x => x.Visita)
        .WithMany(x => x.Reprogramaciones)
        .HasForeignKey(x => new { x.VisitaId, x.EmpresaId })
        .HasPrincipalKey(x => new { x.Id, x.EmpresaId })
        .OnDelete(DeleteBehavior.Restrict);

    entity.HasOne(x => x.Usuario)
    .WithMany()
    .HasForeignKey(x => x.UsuarioId)
    .OnDelete(DeleteBehavior.Restrict);
});


        // INSPECCIONES

        modelBuilder.Entity<Inspection>(entity =>
        {
            entity.HasOne(x => x.Empresa)
                .WithMany()
                .HasForeignKey(x => x.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Visita)
                .WithMany()
                .HasForeignKey(x => new { x.VisitaId, x.EmpresaId })
                .HasPrincipalKey(x => new { x.Id, x.EmpresaId })
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Cultivo)
                .WithMany()
                .HasForeignKey(x => new { x.CultivoId, x.EmpresaId })
                .HasPrincipalKey(x => new { x.Id, x.EmpresaId })
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.CategoriaCalidad)
                .WithMany(x => x.Inspecciones)
                .HasForeignKey(x => new { x.CategoriaCalidadId, x.EmpresaId })
                .HasPrincipalKey(x => new { x.Id, x.EmpresaId })
                .OnDelete(DeleteBehavior.Restrict);
        });

        // CATEGORÍAS DE CALIDAD
        modelBuilder.Entity<QualityCategory>(entity =>
        {
            entity.HasOne(x => x.Empresa)
                .WithMany()
                .HasForeignKey(x => x.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // PARÁMETROS DE EVALUACIÓN

        modelBuilder.Entity<EvaluationParameter>(entity =>
        {
            entity.HasOne(x => x.Empresa)
                .WithMany()
                .HasForeignKey(x => x.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.TipoCultivo)
                .WithMany()
                .HasForeignKey(x => new { x.TipoCultivoId, x.EmpresaId })
                .HasPrincipalKey(x => new { x.Id, x.EmpresaId })
                .OnDelete(DeleteBehavior.Restrict);
        });


        // RESULTADOS DE PARÁMETROS

        modelBuilder.Entity<ParameterResult>(entity =>
        {
            entity.HasOne(x => x.Empresa)
                .WithMany()
                .HasForeignKey(x => x.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Inspeccion)
                .WithMany(x => x.ResultadosParametros)
                .HasForeignKey(x => new { x.InspeccionId, x.EmpresaId })
                .HasPrincipalKey(x => new { x.Id, x.EmpresaId })
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.ParametroEvaluacion)
                .WithMany(x => x.Resultados)
                .HasForeignKey(x => new { x.ParametroEvaluacionId, x.EmpresaId })
                .HasPrincipalKey(x => new { x.Id, x.EmpresaId })
                .OnDelete(DeleteBehavior.Restrict);
        });

        // FERTILIZANTES
        modelBuilder.Entity<Fertilizer>(entity =>
        {
            entity.HasOne(x => x.Empresa)
                .WithMany()
                .HasForeignKey(x => x.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // RECOMENDACIONES DE FERTILIZACIÓN

modelBuilder.Entity<FertilizerRecommendation>(entity =>
{
    entity.HasOne(x => x.Empresa)
        .WithMany()
        .HasForeignKey(x => x.EmpresaId)
        .OnDelete(DeleteBehavior.Restrict);

    entity.HasOne(x => x.Visita)
        .WithMany()
        .HasForeignKey(x => new { x.VisitaId, x.EmpresaId })
        .HasPrincipalKey(x => new { x.Id, x.EmpresaId })
        .OnDelete(DeleteBehavior.Restrict);

    entity.HasOne(x => x.Cultivo)
        .WithMany()
        .HasForeignKey(x => new { x.CultivoId, x.EmpresaId })
        .HasPrincipalKey(x => new { x.Id, x.EmpresaId })
        .OnDelete(DeleteBehavior.Restrict);

    entity.HasOne(x => x.Fertilizante)
        .WithMany(x => x.Recomendaciones)
        .HasForeignKey(x => new { x.FertilizanteId, x.EmpresaId })
        .HasPrincipalKey(x => new { x.Id, x.EmpresaId })
        .OnDelete(DeleteBehavior.Restrict);
});


        // APLICACIONES DE FERTILIZANTES

modelBuilder.Entity<FertilizerApplication>(entity =>
{
    entity.HasOne(x => x.Empresa)
        .WithMany()
        .HasForeignKey(x => x.EmpresaId)
        .OnDelete(DeleteBehavior.Restrict);

    entity.HasOne(x => x.Cultivo)
        .WithMany()
        .HasForeignKey(x => new { x.CultivoId, x.EmpresaId })
        .HasPrincipalKey(x => new { x.Id, x.EmpresaId })
        .OnDelete(DeleteBehavior.Restrict);

    entity.HasOne(x => x.Fertilizante)
        .WithMany(x => x.Aplicaciones)
        .HasForeignKey(x => new { x.FertilizanteId, x.EmpresaId })
        .HasPrincipalKey(x => new { x.Id, x.EmpresaId })
        .OnDelete(DeleteBehavior.Restrict);

    entity.HasOne(x => x.Recomendacion)
        .WithMany()
        .HasForeignKey(x => new { x.RecomendacionId, x.EmpresaId })
        .HasPrincipalKey(x => new { x.Id, x.EmpresaId })
        .OnDelete(DeleteBehavior.Restrict);

    entity.HasOne(x => x.Tecnico)
        .WithMany()
        .HasForeignKey(x => new { x.TecnicoId, x.EmpresaId })
        .HasPrincipalKey(x => new { x.Id, x.EmpresaId })
        .OnDelete(DeleteBehavior.Restrict);
});


        // SUSCRIPCIONES
        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasOne(x => x.Empresa)
                .WithMany()
                .HasForeignKey(x => x.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Plan)
                .WithMany(x => x.Suscripciones)
                .HasForeignKey(x => x.PlanId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // INFORMES

modelBuilder.Entity<Report>(entity =>
{
    entity.HasOne(x => x.Empresa)
        .WithMany()
        .HasForeignKey(x => x.EmpresaId)
        .OnDelete(DeleteBehavior.Restrict);

    entity.HasOne(x => x.Visita)
        .WithMany()
        .HasForeignKey(x => new { x.VisitaId, x.EmpresaId })
        .HasPrincipalKey(x => new { x.Id, x.EmpresaId })
        .OnDelete(DeleteBehavior.Restrict);
});



        // FILTROS GLOBALES POR EMPRESA

        modelBuilder.Entity<Company>()
            .HasQueryFilter(x =>
                _currentUserService.EsSuperAdmin ||
                (EmpresaId != null && x.Id == EmpresaId));

        modelBuilder.Entity<User>()
            .HasQueryFilter(x =>
                _currentUserService.EsSuperAdmin ||
                (EmpresaId != null && x.EmpresaId == EmpresaId));

        modelBuilder.Entity<Producer>()
            .HasQueryFilter(x =>
                _currentUserService.EsSuperAdmin ||
                x.EmpresaId == EmpresaId);

        modelBuilder.Entity<Technician>()
            .HasQueryFilter(x =>
                _currentUserService.EsSuperAdmin ||
                x.EmpresaId == EmpresaId);

        modelBuilder.Entity<Farm>()
            .HasQueryFilter(x =>
                _currentUserService.EsSuperAdmin ||
                x.EmpresaId == EmpresaId);

        modelBuilder.Entity<Crop>()
            .HasQueryFilter(x =>
                _currentUserService.EsSuperAdmin ||
                x.EmpresaId == EmpresaId);

        modelBuilder.Entity<AssistanceRequest>()
            .HasQueryFilter(x =>
                _currentUserService.EsSuperAdmin ||
                x.EmpresaId == EmpresaId);

        modelBuilder.Entity<AssistanceRequestCrop>()
            .HasQueryFilter(x =>
                _currentUserService.EsSuperAdmin ||
                x.EmpresaId == EmpresaId);

        modelBuilder.Entity<Visit>()
            .HasQueryFilter(x =>
                _currentUserService.EsSuperAdmin ||
                x.EmpresaId == EmpresaId);

        modelBuilder.Entity<ReschedulingVisit>()
            .HasQueryFilter(x =>
                _currentUserService.EsSuperAdmin ||
                x.EmpresaId == EmpresaId);

        modelBuilder.Entity<Inspection>()
            .HasQueryFilter(x =>
                _currentUserService.EsSuperAdmin ||
                x.EmpresaId == EmpresaId);

        modelBuilder.Entity<QualityCategory>()
            .HasQueryFilter(x =>
                _currentUserService.EsSuperAdmin ||
                x.EmpresaId == EmpresaId);

        modelBuilder.Entity<EvaluationParameter>()
            .HasQueryFilter(x =>
                _currentUserService.EsSuperAdmin ||
                x.EmpresaId == EmpresaId);

        modelBuilder.Entity<ParameterResult>()
            .HasQueryFilter(x =>
                _currentUserService.EsSuperAdmin ||
                x.EmpresaId == EmpresaId);

        modelBuilder.Entity<Fertilizer>()
            .HasQueryFilter(x =>
                _currentUserService.EsSuperAdmin ||
                x.EmpresaId == EmpresaId);

        modelBuilder.Entity<FertilizerRecommendation>()
            .HasQueryFilter(x =>
                _currentUserService.EsSuperAdmin ||
                x.EmpresaId == EmpresaId);

        modelBuilder.Entity<FertilizerApplication>()
            .HasQueryFilter(x =>
                _currentUserService.EsSuperAdmin ||
                x.EmpresaId == EmpresaId);

        modelBuilder.Entity<Subscription>()
            .HasQueryFilter(x =>
                _currentUserService.EsSuperAdmin ||
                x.EmpresaId == EmpresaId);

        modelBuilder.Entity<Report>()
            .HasQueryFilter(x =>
                _currentUserService.EsSuperAdmin ||
                x.EmpresaId == EmpresaId);

        modelBuilder.Entity<TipoCultivo>()
                .HasQueryFilter(x =>
                _currentUserService.EsSuperAdmin ||
                x.EmpresaId == EmpresaId);

    }
}
