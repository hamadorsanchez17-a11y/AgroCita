
namespace AgroCita.Domain.Entities;

public class Crop
{
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public int FincaId { get; set; }
    public int TipoCultivoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Variedad { get; set; }
    public decimal? AreaSembrada { get; set; }
    public DateOnly? FechaSiembra { get; set; }
    public bool Activo { get; set; }

    public Company? Empresa { get; set; }
    public Farm? Finca { get; set; }
    public TipoCultivo? TipoCultivo { get; set; }

    public ICollection<AssistanceRequestCrop> Solicitudes { get; set; } = new List<AssistanceRequestCrop>();
}
