namespace AgroCita.Domain.Entities;

public class AssistanceRequest
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public int ProductorId { get; set; }

    public int FincaId { get; set; }

    public DateOnly FechaDeseada { get; set; }

    public string Motivo { get; set; } = string.Empty;

    public AssistanceRequestStatus Estado { get; set; }

    public DateTime CreatedAt { get; set; }

    public Producer? Productor { get; set; }

    public Farm? Finca { get; set; }

    public ICollection<AssistanceRequestCrop> Cultivos { get; set; }
        = new List<AssistanceRequestCrop>();

    public ICollection<Visit> Visitas { get; set; }
        = new List<Visit>();
}

public enum AssistanceRequestStatus
{
    PENDIENTE,
    ACEPTADA,
    RECHAZADA,
    CANCELADA
}