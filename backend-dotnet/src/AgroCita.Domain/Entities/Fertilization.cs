using AgroCita.Domain.Common;

namespace AgroCita.Domain.Entities;

public class Fertilizer : ITenantEntity
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Tipo { get; set; } = string.Empty;

    public string UnidadMedida { get; set; } = string.Empty;

    public string? Composicion { get; set; }
}

public class FertilizationRecommendation : ITenantEntity
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public int VisitaId { get; set; }

    public int CultivoId { get; set; }

    public int FertilizanteId { get; set; }

    public decimal DosisRecomendada { get; set; }

    public string? Frecuencia { get; set; }

    public string Indicaciones { get; set; } = string.Empty;

    public DateOnly FechaRecomendacion { get; set; }

    public Visit? Visita { get; set; }

    public Crop? Cultivo { get; set; }

    public Fertilizer? Fertilizante { get; set; }
}

public class FertilizerApplication : ITenantEntity
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public int CultivoId { get; set; }

    public int FertilizanteId { get; set; }

    public int? RecomendacionId { get; set; }

    public int TecnicoId { get; set; }

    public decimal CantidadAplicada { get; set; }

    public DateOnly FechaAplicacion { get; set; }

    public string? Observaciones { get; set; }

    public Crop? Cultivo { get; set; }

    public Fertilizer? Fertilizante { get; set; }

    public FertilizationRecommendation? Recomendacion { get; set; }

    public Technician? Tecnico { get; set; }
}
