using AgroCita.Domain.Common;

namespace AgroCita.Domain.Entities;

public class Crop : ITenantEntity
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

    public Farm? Finca { get; set; }

    public CropType? TipoCultivo { get; set; }
}
