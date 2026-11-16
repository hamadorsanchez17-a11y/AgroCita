using AgroCita.Domain.Common;

namespace AgroCita.Domain.Entities;

public class Producer : ITenantEntity
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public int UsuarioId { get; set; }

    public string Dni { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    public string? Direccion { get; set; }

    public User? Usuario { get; set; }

    public ICollection<Farm> Fincas { get; set; } = new List<Farm>();
}
