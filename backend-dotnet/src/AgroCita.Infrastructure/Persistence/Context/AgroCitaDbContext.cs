
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

    public DbSet<User> Users => Set<User>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Producer> Producers => Set<Producer>();
    public DbSet<Technician> Technicians => Set<Technician>();
    public DbSet<Farm> Farms => Set<Farm>();
    public DbSet<Crop> Crops => Set<Crop>();
    public DbSet<AssistanceRequest> AssistanceRequests => Set<AssistanceRequest>();
    public DbSet<AssistanceRequestCrop> AssistanceRequestCrops => Set<AssistanceRequestCrop>();
    public DbSet<Visit> Visits => Set<Visit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // USUARIOS
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("usuarios");

            entity.HasKey(u => u.Id);

            entity.Property(u => u.EmpresaId);

            entity.Property(u => u.NombreCompleto)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(u => u.PasswordHash)
                .IsRequired();

            entity.Property(u => u.Rol)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(u => u.Activo)
                .IsRequired();

            entity.HasIndex(u => u.Email)
                .IsUnique();
        });

        // CLAVE COMPUESTA DE LA RELACIÓN SOLICITUD-CULTIVO
        modelBuilder.Entity<AssistanceRequestCrop>()
            .HasKey(x => new { x.SolicitudId, x.CultivoId });

        // FILTROS GLOBALES POR EMPRESA

        modelBuilder.Entity<User>()
            .HasQueryFilter(x =>
                EmpresaId != null && x.EmpresaId == EmpresaId);

        modelBuilder.Entity<Company>()
            .HasQueryFilter(x =>
                EmpresaId != null && x.Id == EmpresaId);

        modelBuilder.Entity<AssistanceRequest>()
            .HasQueryFilter(x =>
                EmpresaId != null && x.EmpresaId == EmpresaId);

        modelBuilder.Entity<Producer>()
            .HasQueryFilter(x =>
                EmpresaId != null && x.EmpresaId == EmpresaId);

        modelBuilder.Entity<Technician>()
            .HasQueryFilter(x =>
                EmpresaId != null && x.EmpresaId == EmpresaId);

        modelBuilder.Entity<Visit>()
            .HasQueryFilter(x =>
                EmpresaId != null && x.EmpresaId == EmpresaId);

        // FARMAS: pertenecen a un productor
        modelBuilder.Entity<Farm>()
            .HasQueryFilter(x =>
                EmpresaId != null &&
                x.Productor != null &&
                x.Productor.EmpresaId == EmpresaId);

        // CULTIVOS: pertenecen a una finca
        modelBuilder.Entity<Crop>()
            .HasQueryFilter(x =>
                EmpresaId != null &&
                x.Finca != null &&
                x.Finca.Productor != null &&
                x.Finca.Productor.EmpresaId == EmpresaId);

        // CULTIVOS DE SOLICITUDES: pertenecen a una solicitud
        modelBuilder.Entity<AssistanceRequestCrop>()
            .HasQueryFilter(x =>
                EmpresaId != null &&
                x.Solicitud != null &&
                x.Solicitud.EmpresaId == EmpresaId);
    }
}
