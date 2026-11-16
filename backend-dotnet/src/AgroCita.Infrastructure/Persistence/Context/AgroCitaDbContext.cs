using System.Reflection;
using AgroCita.Application.Common.Interfaces;
using AgroCita.Domain.Common;
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

    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Plan> Plans => Set<Plan>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Producer> Producers => Set<Producer>();
    public DbSet<Technician> Technicians => Set<Technician>();
    public DbSet<Farm> Farms => Set<Farm>();
    public DbSet<CropType> CropTypes => Set<CropType>();
    public DbSet<Crop> Crops => Set<Crop>();
    public DbSet<AssistanceRequest> AssistanceRequests => Set<AssistanceRequest>();
    public DbSet<AssistanceRequestCrop> AssistanceRequestCrops => Set<AssistanceRequestCrop>();
    public DbSet<Visit> Visits => Set<Visit>();
    public DbSet<VisitReschedule> VisitReschedules => Set<VisitReschedule>();
    public DbSet<EvaluationParameter> EvaluationParameters => Set<EvaluationParameter>();
    public DbSet<QualityCategory> QualityCategories => Set<QualityCategory>();
    public DbSet<Inspection> Inspections => Set<Inspection>();
    public DbSet<ParameterResult> ParameterResults => Set<ParameterResult>();
    public DbSet<Fertilizer> Fertilizers => Set<Fertilizer>();
    public DbSet<FertilizationRecommendation> FertilizationRecommendations => Set<FertilizationRecommendation>();
    public DbSet<FertilizerApplication> FertilizerApplications => Set<FertilizerApplication>();
    public DbSet<Report> Reports => Set<Report>();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (EmpresaId is int empresaId)
        {
            foreach (var entry in ChangeTracker.Entries<ITenantEntity>())
            {
                if (entry.State == EntityState.Added && entry.Entity.EmpresaId == 0)
                {
                    entry.Entity.EmpresaId = empresaId;
                }
            }

            foreach (var entry in ChangeTracker.Entries<User>())
            {
                if (entry.State == EntityState.Added && entry.Entity.EmpresaId is null)
                {
                    entry.Entity.EmpresaId = empresaId;
                }
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Rol");
            entity.Property(r => r.Nombre).IsRequired().HasMaxLength(30);
            entity.HasIndex(r => r.Nombre).IsUnique();
            entity.HasData(
                new Role { Id = Domain.Constants.Roles.SuperAdminId, Nombre = Domain.Constants.Roles.SuperAdmin },
                new Role { Id = Domain.Constants.Roles.AdminEmpresaId, Nombre = Domain.Constants.Roles.AdminEmpresa },
                new Role { Id = Domain.Constants.Roles.TecnicoId, Nombre = Domain.Constants.Roles.Tecnico },
                new Role { Id = Domain.Constants.Roles.ProductorId, Nombre = Domain.Constants.Roles.Productor });
        });

        modelBuilder.Entity<Plan>(entity =>
        {
            entity.ToTable("Plan");
            entity.Property(p => p.Nombre).IsRequired().HasMaxLength(60);
            entity.Property(p => p.Descripcion).HasMaxLength(250);
            entity.Property(p => p.PrecioMensual).HasPrecision(10, 2);
            entity.HasIndex(p => p.Nombre).IsUnique();
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Empresa");
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Rtn).IsRequired().HasMaxLength(14).HasColumnName("RTN");
            entity.Property(e => e.Email).IsRequired().HasMaxLength(120);
            entity.Property(e => e.Telefono).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Direccion).HasMaxLength(250);
            entity.Property(e => e.CodigoInvitacion).IsRequired().HasMaxLength(12);
            entity.HasIndex(e => e.Rtn).IsUnique();
            entity.HasIndex(e => e.CodigoInvitacion).IsUnique();
            entity.HasQueryFilter(e => EmpresaId != null && e.Id == EmpresaId);
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.ToTable("Suscripcion");
            entity.Property(s => s.Estado).HasConversion<string>().HasMaxLength(20);
            entity.HasOne(s => s.Empresa).WithMany(e => e.Suscripciones).HasForeignKey(s => s.EmpresaId);
            entity.HasOne(s => s.Plan).WithMany().HasForeignKey(s => s.PlanId);
            entity.HasIndex(s => new { s.EmpresaId, s.Estado });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Usuario");
            entity.Property(u => u.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Apellido).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(120);
            entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);
            entity.HasOne(u => u.Rol).WithMany().HasForeignKey(u => u.RolId);
            entity.HasOne(u => u.Empresa).WithMany().HasForeignKey(u => u.EmpresaId);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasQueryFilter(u => EmpresaId != null && u.EmpresaId == EmpresaId);
        });

        modelBuilder.Entity<Producer>(entity =>
        {
            entity.ToTable("Productor");
            entity.Property(p => p.Dni).IsRequired().HasMaxLength(13).HasColumnName("DNI");
            entity.Property(p => p.Telefono).IsRequired().HasMaxLength(20);
            entity.Property(p => p.Direccion).HasMaxLength(250);
            entity.HasOne(p => p.Usuario).WithMany().HasForeignKey(p => p.UsuarioId);
            entity.HasIndex(p => p.UsuarioId).IsUnique();
            entity.HasIndex(p => new { p.EmpresaId, p.Dni }).IsUnique();
        });

        modelBuilder.Entity<Technician>(entity =>
        {
            entity.ToTable("Tecnico");
            entity.Property(t => t.Especialidad).HasMaxLength(100);
            entity.Property(t => t.Telefono).IsRequired().HasMaxLength(20);
            entity.HasOne(t => t.Usuario).WithMany().HasForeignKey(t => t.UsuarioId);
            entity.HasIndex(t => t.UsuarioId).IsUnique();
        });

        modelBuilder.Entity<Farm>(entity =>
        {
            entity.ToTable("Finca");
            entity.Property(f => f.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(f => f.Departamento).IsRequired().HasMaxLength(60);
            entity.Property(f => f.Municipio).IsRequired().HasMaxLength(60);
            entity.Property(f => f.Ubicacion).IsRequired().HasMaxLength(250);
            entity.Property(f => f.AreaManzanas).HasPrecision(10, 2);
            entity.Property(f => f.Descripcion).HasMaxLength(500);
            entity.HasOne(f => f.Productor).WithMany(p => p.Fincas).HasForeignKey(f => f.ProductorId);
            entity.HasIndex(f => new { f.ProductorId, f.Nombre }).IsUnique();
        });

        modelBuilder.Entity<CropType>(entity =>
        {
            entity.ToTable("TipoCultivo");
            entity.Property(t => t.Nombre).IsRequired().HasMaxLength(60);
            entity.HasIndex(t => new { t.EmpresaId, t.Nombre }).IsUnique();
        });

        modelBuilder.Entity<Crop>(entity =>
        {
            entity.ToTable("Cultivo");
            entity.Property(c => c.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Variedad).HasMaxLength(60);
            entity.Property(c => c.AreaSembrada).HasPrecision(10, 2);
            entity.HasOne(c => c.Finca).WithMany(f => f.Cultivos).HasForeignKey(c => c.FincaId);
            entity.HasOne(c => c.TipoCultivo).WithMany().HasForeignKey(c => c.TipoCultivoId);
        });

        modelBuilder.Entity<AssistanceRequest>(entity =>
        {
            entity.ToTable("Solicitud");
            entity.Property(s => s.Motivo).IsRequired().HasMaxLength(500);
            entity.Property(s => s.Estado).HasConversion<string>().HasMaxLength(20);
            entity.Property(s => s.ComentarioEstado).HasMaxLength(500);
            entity.HasOne(s => s.Finca).WithMany().HasForeignKey(s => s.FincaId);
            entity.HasIndex(s => new { s.EmpresaId, s.Estado });
        });

        modelBuilder.Entity<AssistanceRequestCrop>(entity =>
        {
            entity.ToTable("SolicitudCultivo");
            entity.HasKey(x => new { x.SolicitudId, x.CultivoId });
            entity.HasOne(x => x.Solicitud).WithMany(s => s.Cultivos).HasForeignKey(x => x.SolicitudId);
            entity.HasOne(x => x.Cultivo).WithMany().HasForeignKey(x => x.CultivoId);
        });

        modelBuilder.Entity<Visit>(entity =>
        {
            entity.ToTable("Visita");
            entity.Property(v => v.Estado).HasConversion<string>().HasMaxLength(20);
            entity.Property(v => v.MotivoCancelacion).HasMaxLength(250);
            entity.HasOne(v => v.Solicitud).WithMany(s => s.Visitas).HasForeignKey(v => v.SolicitudId);
            entity.HasOne(v => v.Tecnico).WithMany(t => t.Visitas).HasForeignKey(v => v.TecnicoId);
            entity.HasIndex(v => new { v.TecnicoId, v.FechaProgramada });
            entity.HasIndex(v => new { v.EmpresaId, v.Estado });
        });

        modelBuilder.Entity<VisitReschedule>(entity =>
        {
            entity.ToTable("ReprogramacionVisita");
            entity.Property(r => r.Motivo).IsRequired().HasMaxLength(250);
            entity.HasOne(r => r.Visita).WithMany(v => v.Reprogramaciones).HasForeignKey(r => r.VisitaId);
            entity.HasOne(r => r.Usuario).WithMany().HasForeignKey(r => r.UsuarioId);
        });

        modelBuilder.Entity<EvaluationParameter>(entity =>
        {
            entity.ToTable("ParametroEvaluacion");
            entity.Property(p => p.Nombre).IsRequired().HasMaxLength(80);
            entity.Property(p => p.Unidad).HasMaxLength(20);
            entity.Property(p => p.ValorMinimo).HasPrecision(10, 2);
            entity.Property(p => p.ValorMaximo).HasPrecision(10, 2);
            entity.HasOne(p => p.TipoCultivo).WithMany().HasForeignKey(p => p.TipoCultivoId);
            entity.HasIndex(p => new { p.TipoCultivoId, p.Nombre }).IsUnique();
        });

        modelBuilder.Entity<QualityCategory>(entity =>
        {
            entity.ToTable("CategoriaCalidad");
            entity.Property(c => c.Nombre).IsRequired().HasMaxLength(60);
            entity.Property(c => c.Descripcion).HasMaxLength(250);
            entity.HasIndex(c => new { c.EmpresaId, c.Nombre }).IsUnique();
        });

        modelBuilder.Entity<Inspection>(entity =>
        {
            entity.ToTable("Inspeccion");
            entity.HasOne(i => i.Visita).WithMany(v => v.Inspecciones).HasForeignKey(i => i.VisitaId);
            entity.HasOne(i => i.Cultivo).WithMany().HasForeignKey(i => i.CultivoId);
            entity.HasOne(i => i.CategoriaCalidad).WithMany().HasForeignKey(i => i.CategoriaCalidadId);
            entity.HasIndex(i => new { i.VisitaId, i.CultivoId }).IsUnique();
            entity.HasIndex(i => new { i.CultivoId, i.FechaInspeccion });
        });

        modelBuilder.Entity<ParameterResult>(entity =>
        {
            entity.ToTable("ResultadoParametro");
            entity.Property(r => r.Valor).IsRequired().HasMaxLength(100);
            entity.HasOne(r => r.Inspeccion).WithMany(i => i.Resultados).HasForeignKey(r => r.InspeccionId);
            entity.HasOne(r => r.ParametroEvaluacion).WithMany().HasForeignKey(r => r.ParametroEvaluacionId);
            entity.HasIndex(r => new { r.InspeccionId, r.ParametroEvaluacionId }).IsUnique();
        });

        modelBuilder.Entity<Fertilizer>(entity =>
        {
            entity.ToTable("Fertilizante");
            entity.Property(f => f.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(f => f.Tipo).IsRequired().HasMaxLength(40);
            entity.Property(f => f.UnidadMedida).IsRequired().HasMaxLength(20);
            entity.Property(f => f.Composicion).HasMaxLength(250);
            entity.HasIndex(f => new { f.EmpresaId, f.Nombre }).IsUnique();
        });

        modelBuilder.Entity<FertilizationRecommendation>(entity =>
        {
            entity.ToTable("RecomendacionFertilizacion");
            entity.Property(r => r.DosisRecomendada).HasPrecision(10, 2);
            entity.Property(r => r.Frecuencia).HasMaxLength(100);
            entity.Property(r => r.Indicaciones).IsRequired().HasMaxLength(500);
            entity.HasOne(r => r.Visita).WithMany().HasForeignKey(r => r.VisitaId);
            entity.HasOne(r => r.Cultivo).WithMany().HasForeignKey(r => r.CultivoId);
            entity.HasOne(r => r.Fertilizante).WithMany().HasForeignKey(r => r.FertilizanteId);
        });

        modelBuilder.Entity<FertilizerApplication>(entity =>
        {
            entity.ToTable("AplicacionFertilizante");
            entity.Property(a => a.CantidadAplicada).HasPrecision(10, 2);
            entity.Property(a => a.Observaciones).HasMaxLength(250);
            entity.HasOne(a => a.Cultivo).WithMany().HasForeignKey(a => a.CultivoId);
            entity.HasOne(a => a.Fertilizante).WithMany().HasForeignKey(a => a.FertilizanteId);
            entity.HasOne(a => a.Recomendacion).WithMany().HasForeignKey(a => a.RecomendacionId);
            entity.HasOne(a => a.Tecnico).WithMany().HasForeignKey(a => a.TecnicoId);
            entity.HasIndex(a => new { a.CultivoId, a.FechaAplicacion });
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.ToTable("Informe");
            entity.Property(i => i.Observaciones).IsRequired();
            entity.Property(i => i.Diagnostico).IsRequired();
            entity.Property(i => i.Recomendaciones).IsRequired();
            entity.HasOne(i => i.Visita).WithOne(v => v.Informe).HasForeignKey<Report>(i => i.VisitaId);
        });

        var metodoFiltro = typeof(AgroCitaDbContext).GetMethod(
            nameof(ConfigurarEntidadDeEmpresa),
            BindingFlags.Instance | BindingFlags.NonPublic)!;

        var entidadesDeEmpresa = modelBuilder.Model.GetEntityTypes()
            .Select(t => t.ClrType)
            .Where(t => typeof(ITenantEntity).IsAssignableFrom(t))
            .ToList();

        foreach (var tipo in entidadesDeEmpresa)
        {
            metodoFiltro.MakeGenericMethod(tipo).Invoke(this, [modelBuilder]);
        }

        foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(t => t.GetForeignKeys()))
        {
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }

    private void ConfigurarEntidadDeEmpresa<T>(ModelBuilder modelBuilder)
        where T : class, ITenantEntity
    {
        var entity = modelBuilder.Entity<T>();

        entity.HasQueryFilter(x => EmpresaId != null && x.EmpresaId == EmpresaId);

        if (typeof(T) != typeof(Subscription))
        {
            entity.HasOne<Company>().WithMany().HasForeignKey(nameof(ITenantEntity.EmpresaId));
        }
    }
}
