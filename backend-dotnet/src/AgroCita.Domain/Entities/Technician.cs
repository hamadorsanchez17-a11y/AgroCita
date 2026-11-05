namespace AgroCita.Domain.Entities;

public class Technician
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public int UsuarioId { get; set; }

    public string Especialidad { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    public bool Activo { get; set; }

    public ICollection<Visit> Visitas { get; set; }
        = new List<Visit>();
}