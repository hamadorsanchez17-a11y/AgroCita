using AgroCita.Domain.Common;

namespace AgroCita.Domain.Entities;

public class Visit : ITenantEntity
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public int SolicitudId { get; set; }

    public int TecnicoId { get; set; }

    public DateTime FechaProgramada { get; set; }

    public VisitStatus Estado { get; set; }

    public string? MotivoCancelacion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public AssistanceRequest? Solicitud { get; set; }

    public Technician? Tecnico { get; set; }

    public Report? Informe { get; set; }

    public ICollection<VisitReschedule> Reprogramaciones { get; set; }
        = new List<VisitReschedule>();

    public ICollection<Inspection> Inspecciones { get; set; }
        = new List<Inspection>();

    public bool EstaCerrada =>
        Estado is VisitStatus.Completada or VisitStatus.Cancelada;
}

public enum VisitStatus
{
    Pendiente,
    Confirmada,
    EnProceso,
    Completada,
    Cancelada
}
