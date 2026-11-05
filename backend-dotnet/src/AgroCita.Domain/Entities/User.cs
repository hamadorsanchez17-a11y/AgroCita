namespace AgroCita.Domain.Entities;

public class User
{
    public int Id { get; set; }

    public int? EmpresaId { get; set; }

    public string NombreCompleto { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Rol { get; set; } = string.Empty;

    public bool Activo { get; set; }
}