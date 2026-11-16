using AgroCita.Domain.Common;

namespace AgroCita.Domain.Entities;

public class AssistanceRequest : ITenantEntity
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public int FincaId { get; set; }

    public string Motivo { get; set; } = string.Empty;

    public DateOnly FechaDeseada { get; set; }

    public AssistanceRequestStatus Estado { get; set; }

    public string? ComentarioEstado { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Farm? Finca { get; set; }

    public ICollection<AssistanceRequestCrop> Cultivos { get; set; }
        = new List<AssistanceRequestCrop>();

    public ICollection<Visit> Visitas { get; set; }
        = new List<Visit>();
}

public enum AssistanceRequestStatus
{
    Pendiente,
    EnAtencion,
    Atendida,
    Rechazada
}
