using AgroCita.Domain.Common;

namespace AgroCita.Domain.Entities;

public class EvaluationParameter : ITenantEntity
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public int TipoCultivoId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Unidad { get; set; }

    public decimal? ValorMinimo { get; set; }

    public decimal? ValorMaximo { get; set; }

    public CropType? TipoCultivo { get; set; }
}

public class QualityCategory : ITenantEntity
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }
}

public class Inspection : ITenantEntity
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public int VisitaId { get; set; }

    public int CultivoId { get; set; }

    public int? CategoriaCalidadId { get; set; }

    public DateTime FechaInspeccion { get; set; }

    public string? Observaciones { get; set; }

    public Visit? Visita { get; set; }

    public Crop? Cultivo { get; set; }

    public QualityCategory? CategoriaCalidad { get; set; }

    public ICollection<ParameterResult> Resultados { get; set; }
        = new List<ParameterResult>();
}

public class ParameterResult : ITenantEntity
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public int InspeccionId { get; set; }

    public int ParametroEvaluacionId { get; set; }

    public string Valor { get; set; } = string.Empty;

    public Inspection? Inspeccion { get; set; }

    public EvaluationParameter? ParametroEvaluacion { get; set; }
}
