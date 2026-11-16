using AgroCita.Domain.Common;

namespace AgroCita.Domain.Entities;

public class CropType : ITenantEntity
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public string Nombre { get; set; } = string.Empty;
}
