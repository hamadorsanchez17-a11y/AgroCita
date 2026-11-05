namespace AgroCita.Domain.Entities;

public class Crop
{
    public int Id { get; set; }

    public int FincaId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public Farm? Finca { get; set; }
}