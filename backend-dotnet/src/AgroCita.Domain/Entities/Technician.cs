using AgroCita.Domain.Common;

namespace AgroCita.Domain.Entities;

public class Technician : ITenantEntity
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public int UsuarioId { get; set; }

    public string? Especialidad { get; set; }

    public string Telefono { get; set; } = string.Empty;

    public bool Activo { get; set; }

    public User? Usuario { get; set; }

    public ICollection<Visit> Visitas { get; set; }
        = new List<Visit>();
}
