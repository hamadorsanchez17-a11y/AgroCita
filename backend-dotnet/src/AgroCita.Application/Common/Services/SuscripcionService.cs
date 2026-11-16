using AgroCita.Application.Common.Exceptions;
using AgroCita.Application.Common.Interfaces;
using AgroCita.Application.Common.Models;
using AgroCita.Domain.Entities;

namespace AgroCita.Application.Common.Services;

public class SuscripcionService(
    IRepository<Subscription> suscripciones,
    IRepository<Technician> tecnicos,
    IRepository<Producer> productores)
{
    public async Task<Subscription?> VigenteAsync(CancellationToken ct)
    {
        var lista = await suscripciones.ListAsync(
            ordenar: q => q.OrderByDescending(s => s.Id),
            tomar: 1,
            incluir: ["Plan", "Empresa"],
            ct: ct);

        return lista.FirstOrDefault();
    }

    public async Task ExigirActivaAsync(CancellationToken ct)
    {
        var vigente = await VigenteAsync(ct);

        if (vigente is null ||
            vigente.Empresa is not { Activa: true } ||
            !vigente.PermiteOperar(Fechas.Hoy))
        {
            throw new ForbiddenException(
                "La suscripción de la organización no está activa.");
        }
    }

    public async Task ExigirCupoDeTecnicosAsync(CancellationToken ct)
    {
        var plan = (await VigenteAsync(ct))?.Plan;

        if (plan?.MaxTecnicos is int maximo &&
            await tecnicos.CountAsync(ct: ct) >= maximo)
        {
            throw new ConflictException(
                $"El plan {plan.Nombre} permite hasta {maximo} técnicos. Cambie de plan para crear más.");
        }
    }

    public async Task ExigirCupoDeProductoresAsync(Company empresa, CancellationToken ct)
    {
        var plan = empresa.SuscripcionVigente()?.Plan;
        var empresaId = empresa.Id;

        if (plan?.MaxProductores is int maximo &&
            await productores.CountAsync(
                p => p.EmpresaId == empresaId, sinFiltroEmpresa: true, ct) >= maximo)
        {
            throw new ConflictException(
                "La organización alcanzó el límite de productores de su plan.");
        }
    }
}
