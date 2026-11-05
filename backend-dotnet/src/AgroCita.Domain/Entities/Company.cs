namespace AgroCita.Domain.Entities;

public class Company
{
    public int Id { get; set; }

    public string CodigoInvitacion { get; set; } = string.Empty;

    public bool Activa { get; set; }
}