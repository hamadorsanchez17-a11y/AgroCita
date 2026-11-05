namespace AgroCita.Domain.Entities;

public class Producer
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public int UsuarioId { get; set; }

    public string NombreCompleto { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Dni { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    public string? Direccion { get; set; }
}