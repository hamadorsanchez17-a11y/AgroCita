
namespace AgroCita.Domain.Entities;

public class Technician
{
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public int UsuarioId { get; set; }
    public string? Especialidad { get; set; }
    public string Telefono { get; set; } = string.Empty;
    public bool Activo { get; set; }

    public Company? Empresa { get; set; }
    public User? Usuario { get; set; }

    public ICollection<Visit> Visitas { get; set; } = new List<Visit>();
}
