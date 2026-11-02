using AgroCita.Domain.Entities.User;

namespace AgroCita.Domain.Entities.Company;

public class Empresa
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string RTN { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    public string? Direccion { get; set; }

    public string CodigoInvitacion { get; set; } = string.Empty;

    public bool Activa { get; set; }

    public DateTime FechaRegistro { get; set; }

    // Navegación
    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}