
namespace AgroCita.Domain.Entities;

public class Farm
{
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public int ProductorId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Departamento { get; set; } = string.Empty;
    public string Municipio { get; set; } = string.Empty;
    public string? Direccion { get; set; }
    public decimal? AreaManzanas { get; set; }
    public string? Descripcion { get; set; }

    public Company? Empresa { get; set; }
    public Producer? Productor { get; set; }

    public ICollection<Crop> Cultivos { get; set; } = new List<Crop>();
    public ICollection<AssistanceRequest> Solicitudes { get; set; } = new List<AssistanceRequest>();
}
