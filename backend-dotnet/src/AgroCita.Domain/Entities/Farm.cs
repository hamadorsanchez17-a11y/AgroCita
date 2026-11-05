namespace AgroCita.Domain.Entities;

public class Farm
{
    public int Id { get; set; }

    public int ProductorId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Departamento { get; set; } = string.Empty;

    public string Municipio { get; set; } = string.Empty;

    public string Ubicacion { get; set; } = string.Empty;

    public decimal AreaManzanas { get; set; }

    public string? Descripcion { get; set; }

    public Producer? Productor { get; set; }

    public ICollection<Crop> Cultivos { get; set; } = new List<Crop>();
}