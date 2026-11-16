using AgroCita.Domain.Common;

namespace AgroCita.Domain.Entities;

public class Subscription : ITenantEntity
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public int PlanId { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly? FechaFin { get; set; }

    public SubscriptionStatus Estado { get; set; }

    public Company? Empresa { get; set; }

    public Plan? Plan { get; set; }

    public bool PermiteOperar(DateOnly hoy) =>
        Estado == SubscriptionStatus.Activa &&
        (FechaFin is null || FechaFin >= hoy);
}

public enum SubscriptionStatus
{
    Activa,
    Vencida,
    Suspendida,
    Cancelada
}
