
namespace AgroCita.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public int? EmpresaId { get; set; }
    public int RolId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }

    public Company? Empresa { get; set; }
    public Rol? Rol { get; set; }

    public Producer? Productor { get; set; }
    public Technician? Tecnico { get; set; }
}
