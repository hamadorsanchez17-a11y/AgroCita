
namespace AgroCita.Domain.Entities;

public class QualityCategory
{
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }

    public Company? Empresa { get; set; }

    public ICollection<Inspection> Inspecciones { get; set; } = new List<Inspection>();
}
