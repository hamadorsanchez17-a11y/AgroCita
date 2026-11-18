
namespace AgroCita.Domain.Entities;

public class Company
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

    public ICollection<User> Usuarios { get; set; } = new List<User>();
    public ICollection<Producer> Productores { get; set; } = new List<Producer>();
    public ICollection<Technician> Tecnicos { get; set; } = new List<Technician>();
    public ICollection<Farm> Fincas { get; set; } = new List<Farm>();
    public ICollection<Crop> Cultivos { get; set; } = new List<Crop>();
}
