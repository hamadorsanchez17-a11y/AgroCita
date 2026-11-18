
namespace AgroCita.Domain.Entities;

public class TipoCultivo
{
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public string Nombre { get; set; } = string.Empty;

    public Company? Empresa { get; set; }

    public ICollection<Crop> Cultivos { get; set; } = new List<Crop>();
}
