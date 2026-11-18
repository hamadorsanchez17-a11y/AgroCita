
namespace AgroCita.Domain.Entities;

public class ParameterResult
{
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public int InspeccionId { get; set; }
    public int ParametroEvaluacionId { get; set; }
    public string Valor { get; set; } = string.Empty;

    public Company? Empresa { get; set; }
    public Inspection? Inspeccion { get; set; }
    public EvaluationParameter? ParametroEvaluacion { get; set; }
}
