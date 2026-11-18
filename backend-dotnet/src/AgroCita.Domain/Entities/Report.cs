
namespace AgroCita.Domain.Entities;

public class Report
{
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public int VisitaId { get; set; }
    public string? Observaciones { get; set; }
    public string? Diagnostico { get; set; }
    public string? Recomendaciones { get; set; }
    public DateTime FechaElaboracion { get; set; }

    public Company? Empresa { get; set; }
    public Visit? Visita { get; set; }
}
