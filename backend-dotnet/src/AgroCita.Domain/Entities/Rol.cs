
namespace AgroCita.Domain.Entities;

public class Rol
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;

    public ICollection<User> Usuarios { get; set; } = new List<User>();
}
