using AgroCita.Domain.Entities.Company;

namespace AgroCita.Domain.Entities.User;

public class Usuario
{
    public int Id { get; set; }

    public int? EmpresaId { get; set; }

    public int RolId { get; set; }

    public string NombreCompleto { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    // Navegación
    public Empresa? Empresa { get; set; }

    public Rol Rol { get; set; } = null!;
}