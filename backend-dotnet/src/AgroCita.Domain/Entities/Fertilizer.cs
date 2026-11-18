
namespace AgroCita.Domain.Entities;

public class Fertilizer
{
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string? Composicion { get; set; }

    public Company? Empresa { get; set; }

    public ICollection<FertilizerRecommendation> Recomendaciones { get; set; } = new List<FertilizerRecommendation>();
    public ICollection<FertilizerApplication> Aplicaciones { get; set; } = new List<FertilizerApplication>();
}
