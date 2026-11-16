using AgroCita.Domain.Common;

namespace AgroCita.Domain.Entities;

public class Report : ITenantEntity
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public int VisitaId { get; set; }

    public string Observaciones { get; set; } = string.Empty;

    public string Diagnostico { get; set; } = string.Empty;

    public string Recomendaciones { get; set; } = string.Empty;

    public DateTime FechaElaboracion { get; set; }

    public Visit? Visita { get; set; }
}
