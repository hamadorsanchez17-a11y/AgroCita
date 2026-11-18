
namespace AgroCita.Domain.Entities;

public class AssistanceRequest
{
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public int FincaId { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public DateOnly FechaDeseada { get; set; }
    public string Estado { get; set; } = string.Empty;
    public DateTime? ComentarioTecnico { get; set; }
    public DateTime FechaCreacion { get; set; }

    public Company? Empresa { get; set; }
    public Farm? Finca { get; set; }

    public ICollection<AssistanceRequestCrop> Cultivos { get; set; } = new List<AssistanceRequestCrop>();
    public ICollection<Visit> Visitas { get; set; } = new List<Visit>();
}
