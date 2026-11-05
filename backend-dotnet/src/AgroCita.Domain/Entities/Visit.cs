namespace AgroCita.Domain.Entities;

public class Visit
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public int SolicitudId { get; set; }

    public int TecnicoId { get; set; }

    public int FincaId { get; set; }

    public DateOnly FechaProgramada { get; set; }

    public TimeOnly HoraProgramada { get; set; }

    public VisitStatus Estado { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public AssistanceRequest? Solicitud { get; set; }

    public Farm? Finca { get; set; }

    public Technician? Tecnico { get; set; }
}

public enum VisitStatus
{
    PENDIENTE,
    CONFIRMADA,
    EN_PROCESO,
    COMPLETADA,
    CANCELADA
}