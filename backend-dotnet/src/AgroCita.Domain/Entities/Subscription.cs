
namespace AgroCita.Domain.Entities;

public class Subscription
{
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public int PlanId { get; set; }
    public DateOnly FechaInicio { get; set; }
    public DateOnly FechaFin { get; set; }
    public string Estado { get; set; } = string.Empty;

    public Company? Empresa { get; set; }
    public SubscriptionPlan? Plan { get; set; }
}
