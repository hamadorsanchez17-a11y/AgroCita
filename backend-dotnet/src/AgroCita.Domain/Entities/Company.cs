namespace AgroCita.Domain.Entities;

public class Company
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Rtn { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    public string? Direccion { get; set; }

    public string CodigoInvitacion { get; set; } = string.Empty;

    public bool Activa { get; set; }

    public DateTime FechaRegistro { get; set; }

    public ICollection<Subscription> Suscripciones { get; set; }
        = new List<Subscription>();

    public Subscription? SuscripcionVigente() =>
        Suscripciones.OrderByDescending(s => s.Id).FirstOrDefault();
}
