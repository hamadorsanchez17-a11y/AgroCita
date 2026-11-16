using AgroCita.Application.Common.Exceptions;
using AgroCita.Application.Common.Interfaces;
using AgroCita.Application.Common.Models;
using AgroCita.Application.Features.Empresa;
using AgroCita.Domain.Constants;
using AgroCita.Domain.Entities;
using FluentValidation;

namespace AgroCita.Application.Features.Admin;

public class AdminHandler(
    IRepository<Plan> planes,
    IRepository<Company> empresas,
    IRepository<Subscription> suscripciones,
    IRepository<User> usuarios,
    IUnitOfWork unitOfWork,
    IPasswordHashService passwordHashService,
    IValidator<PlanRequestDto> planValidator,
    IValidator<CrearEmpresaDto> empresaValidator,
    IValidator<AsignarPlanDto> asignarValidator,
    IValidator<CambiarEstadoSuscripcionDto> estadoValidator)
{
    public async Task<IReadOnlyList<PlanDto>> ListarPlanesAsync(CancellationToken ct)
    {
        var lista = await planes.ListAsync(ordenar: q => q.OrderBy(p => p.PrecioMensual), ct: ct);

        return lista.Select(p => p.ADto()).ToList();
    }

    public async Task<PlanDto> CrearPlanAsync(PlanRequestDto dto, CancellationToken ct)
    {
        await planValidator.ValidateAndThrowAsync(dto, ct);
        await ExigirNombreDePlanLibreAsync(dto.Nombre.Trim(), 0, ct);

        var plan = new Plan();
        Aplicar(plan, dto);

        planes.Add(plan);
        await unitOfWork.SaveChangesAsync(ct);

        return plan.ADto();
    }

    public async Task<PlanDto> ActualizarPlanAsync(int id, PlanRequestDto dto, CancellationToken ct)
    {
        await planValidator.ValidateAndThrowAsync(dto, ct);

        var plan = await planes.FirstAsync(p => p.Id == id, ct: ct)
            ?? throw new NotFoundException("El plan no existe.");

        await ExigirNombreDePlanLibreAsync(dto.Nombre.Trim(), id, ct);

        Aplicar(plan, dto);
        await unitOfWork.SaveChangesAsync(ct);

        return plan.ADto();
    }

    public async Task<PaginadoDto<EmpresaResumenDto>> ListarEmpresasAsync(
        int? pagina, int? tamano, CancellationToken ct)
    {
        var (numeroPagina, tamanoPagina, saltar) = Paginacion.Normalizar(pagina, tamano);

        var total = await empresas.CountAsync(sinFiltroEmpresa: true, ct: ct);

        var lista = await empresas.ListAsync(
            ordenar: q => q.OrderBy(e => e.Nombre),
            saltar: saltar,
            tomar: tamanoPagina,
            incluir: ["Suscripciones.Plan"],
            sinFiltroEmpresa: true,
            ct: ct);

        return new PaginadoDto<EmpresaResumenDto>(
            lista.Select(e => e.AResumenDto()).ToList(),
            total,
            numeroPagina,
            tamanoPagina);
    }

    public async Task<EmpresaDto> CrearEmpresaAsync(CrearEmpresaDto dto, CancellationToken ct)
    {
        await empresaValidator.ValidateAndThrowAsync(dto, ct);

        var rtn = dto.Rtn;
        var emailAdmin = dto.Admin!.Email.Trim().ToLowerInvariant();

        if (await empresas.AnyAsync(e => e.Rtn == rtn, sinFiltroEmpresa: true, ct))
        {
            throw new ConflictException("Ya existe una organización con ese RTN.");
        }

        if (await usuarios.AnyAsync(u => u.Email == emailAdmin, sinFiltroEmpresa: true, ct))
        {
            throw new ConflictException("Ya existe una cuenta con el correo del administrador.");
        }

        var plan = await PlanActivoAsync(dto.PlanId, ct);

        var empresa = new Company
        {
            Nombre = dto.Nombre.Trim(),
            Rtn = rtn,
            Email = dto.Email.Trim().ToLowerInvariant(),
            Telefono = dto.Telefono,
            Direccion = dto.Direccion?.Trim(),
            CodigoInvitacion = await EmpresaHandler.GenerarCodigoAsync(empresas, ct),
            Activa = true,
            FechaRegistro = Fechas.Ahora,
            Suscripciones =
            [
                new Subscription
                {
                    Plan = plan,
                    FechaInicio = Fechas.Hoy,
                    Estado = SubscriptionStatus.Activa
                }
            ]
        };

        usuarios.Add(new User
        {
            Empresa = empresa,
            RolId = Roles.AdminEmpresaId,
            Nombre = dto.Admin.Nombre.Trim(),
            Apellido = dto.Admin.Apellido?.Trim() ?? string.Empty,
            Email = emailAdmin,
            PasswordHash = passwordHashService.HashPassword(dto.Admin.Password),
            Activo = true,
            FechaCreacion = Fechas.Ahora
        });

        await unitOfWork.SaveChangesAsync(ct);

        return empresa.ADto();
    }

    public async Task<SuscripcionDto> AsignarPlanAsync(
        int empresaId, AsignarPlanDto dto, CancellationToken ct)
    {
        await asignarValidator.ValidateAndThrowAsync(dto, ct);

        var empresa = await empresas.FirstAsync(
                e => e.Id == empresaId, ["Suscripciones"], sinFiltroEmpresa: true, ct)
            ?? throw new NotFoundException("La organización no existe.");

        var plan = await PlanActivoAsync(dto.PlanId, ct);

        foreach (var anterior in empresa.Suscripciones
                     .Where(s => s.Estado == SubscriptionStatus.Activa))
        {
            anterior.Estado = SubscriptionStatus.Cancelada;
        }

        var suscripcion = new Subscription
        {
            Empresa = empresa,
            Plan = plan,
            FechaInicio = dto.FechaInicio!.Value,
            FechaFin = dto.FechaFin,
            Estado = SubscriptionStatus.Activa
        };

        suscripciones.Add(suscripcion);
        await unitOfWork.SaveChangesAsync(ct);

        return suscripcion.ADto();
    }

    public async Task<SuscripcionDto> CambiarEstadoSuscripcionAsync(
        int id, CambiarEstadoSuscripcionDto dto, CancellationToken ct)
    {
        await estadoValidator.ValidateAndThrowAsync(dto, ct);

        var suscripcion = await suscripciones.FirstAsync(
                s => s.Id == id, ["Empresa", "Plan"], sinFiltroEmpresa: true, ct)
            ?? throw new NotFoundException("La suscripción no existe.");

        suscripcion.Estado = dto.Estado!.Value;
        await unitOfWork.SaveChangesAsync(ct);

        return suscripcion.ADto();
    }

    private async Task<Plan> PlanActivoAsync(int id, CancellationToken ct)
    {
        var plan = await planes.FirstAsync(p => p.Id == id, ct: ct);

        if (plan is null || !plan.Activo)
        {
            throw Errores.Campo("PlanId", "El plan no existe o no está activo.");
        }

        return plan;
    }

    private async Task ExigirNombreDePlanLibreAsync(string nombre, int planId, CancellationToken ct)
    {
        var nombreMinusculas = nombre.ToLower();

        if (await planes.AnyAsync(
                p => p.Nombre.ToLower() == nombreMinusculas && p.Id != planId, ct: ct))
        {
            throw new ConflictException("Ya existe un plan con ese nombre.");
        }
    }

    private static void Aplicar(Plan plan, PlanRequestDto dto)
    {
        plan.Nombre = dto.Nombre.Trim();
        plan.Descripcion = dto.Descripcion?.Trim();
        plan.PrecioMensual = dto.PrecioMensual;
        plan.MaxTecnicos = dto.MaxTecnicos;
        plan.MaxProductores = dto.MaxProductores;
        plan.Activo = dto.Activo ?? true;
    }
}
