
namespace AgroCita.Domain.Entities;

public class Visit
{
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public int SolicitudId { get; set; }
    public int TecnicoId { get; set; }
    public DateTime FechaProgramada { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string? MotivoCancelacion { get; set; }
    public DateTime FechaCreacion { get; set; }

    public Company? Empresa { get; set; }
    public AssistanceRequest? Solicitud { get; set; }
    public Technician? Tecnico { get; set; }

    public ICollection<ReschedulingVisit> Reprogramaciones { get; set; } = new List<ReschedulingVisit>();
}
