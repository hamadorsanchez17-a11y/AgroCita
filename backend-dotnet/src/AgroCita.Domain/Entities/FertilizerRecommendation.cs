
namespace AgroCita.Domain.Entities;

public class FertilizerRecommendation
{
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public int VisitaId { get; set; }
    public int CultivoId { get; set; }
    public int FertilizanteId { get; set; }
    public decimal Dosis { get; set; }
    public string UnidadMedida { get; set; } = string.Empty;
    public string? Frecuencia { get; set; }
    public string? Indicaciones { get; set; }
    public DateOnly FechaRecomendacion { get; set; }

    public Company? Empresa { get; set; }
    public Visit? Visita { get; set; }
    public Crop? Cultivo { get; set; }
    public Fertilizer? Fertilizante { get; set; }
}
