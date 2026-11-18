
namespace AgroCita.Domain.Entities;

public class Producer
{
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public int UsuarioId { get; set; }
    public string DNI { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;

    public Company? Empresa { get; set; }
    public User? Usuario { get; set; }

    public ICollection<Farm> Fincas { get; set; } = new List<Farm>();
}
