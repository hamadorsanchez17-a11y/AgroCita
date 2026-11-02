namespace AgroCita.Domain.Entities.User;

public class Rol
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    // Navegación
    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}