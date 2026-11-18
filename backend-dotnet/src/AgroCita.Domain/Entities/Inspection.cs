
namespace AgroCita.Domain.Entities;

public class Inspection
{
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public int VisitaId { get; set; }
    public int CultivoId { get; set; }
    public int CategoriaCalidadId { get; set; }
    public DateTime FechaInspeccion { get; set; }
    public string? Observaciones { get; set; }

    public Company? Empresa { get; set; }
    public Visit? Visita { get; set; }
    public Crop? Cultivo { get; set; }
    public QualityCategory? CategoriaCalidad { get; set; }

    public ICollection<ParameterResult> ResultadosParametros { get; set; } = new List<ParameterResult>();
}
