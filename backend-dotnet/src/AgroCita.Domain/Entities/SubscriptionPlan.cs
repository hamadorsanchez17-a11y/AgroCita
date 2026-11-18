
namespace AgroCita.Domain.Entities;

public class SubscriptionPlan
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal PrecioMensual { get; set; }
    public int MaxTecnicos { get; set; }
    public int MaxProductores { get; set; }
    public bool Activo { get; set; }

    public ICollection<Subscription> Suscripciones { get; set; } = new List<Subscription>();
}
