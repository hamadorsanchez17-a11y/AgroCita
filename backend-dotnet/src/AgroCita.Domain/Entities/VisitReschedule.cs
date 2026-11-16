using AgroCita.Domain.Common;

namespace AgroCita.Domain.Entities;

public class VisitReschedule : ITenantEntity
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public int VisitaId { get; set; }

    public int UsuarioId { get; set; }

    public DateTime FechaAnterior { get; set; }

    public DateTime FechaNueva { get; set; }

    public string Motivo { get; set; } = string.Empty;

    public DateTime FechaRegistro { get; set; }

    public Visit? Visita { get; set; }

    public User? Usuario { get; set; }
}
