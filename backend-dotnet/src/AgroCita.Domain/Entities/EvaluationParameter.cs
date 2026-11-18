
namespace AgroCita.Domain.Entities;

public class EvaluationParameter
{
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public int TipoCultivoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Unidad { get; set; } = string.Empty;
    public decimal ValorMinimo { get; set; }
    public decimal ValorMaximo { get; set; }

    public Company? Empresa { get; set; }
    public TipoCultivo? TipoCultivo { get; set; }

    public ICollection<ParameterResult> Resultados { get; set; } = new List<ParameterResult>();
}
