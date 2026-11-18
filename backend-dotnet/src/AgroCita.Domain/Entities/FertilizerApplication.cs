
namespace AgroCita.Domain.Entities;

public class FertilizerApplication
{
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public int CultivoId { get; set; }
    public int FertilizanteId { get; set; }
    public int RecomendacionId { get; set; }
    public int TecnicoId { get; set; }
    public decimal Cantidad { get; set; }
    public string UnidadMedida { get; set; } = string.Empty;
    public DateOnly FechaAplicacion { get; set; }
    public string? Observaciones { get; set; }

    public Company? Empresa { get; set; }
    public Crop? Cultivo { get; set; }
    public Fertilizer? Fertilizante { get; set; }
    public FertilizerRecommendation? Recomendacion { get; set; }
    public Technician? Tecnico { get; set; }
}
