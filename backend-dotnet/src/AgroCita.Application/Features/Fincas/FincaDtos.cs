using AgroCita.Application.Common.Models;
using AgroCita.Domain.Entities;

namespace AgroCita.Application.Features.Fincas;

public record FincaRequestDto(
    string Nombre,
    string Departamento,
    string Municipio,
    string Direccion,
    decimal? AreaManzanas,
    string? Descripcion);

public record FincaDto
{
    public int Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string Departamento { get; init; } = string.Empty;
    public string Municipio { get; init; } = string.Empty;
    public string Direccion { get; init; } = string.Empty;
    public decimal? AreaManzanas { get; init; }
    public string? Descripcion { get; init; }
    public int CantidadCultivos { get; init; }
}

public record FincaDetalleDto : FincaDto
{
    public ReferenciaDto Productor { get; init; } = new(0, string.Empty);
    public IReadOnlyList<CultivoDto> Cultivos { get; init; } = [];
}

public record CultivoRequestDto(
    int TipoCultivoId,
    string? Nombre,
    string? Variedad,
    decimal? AreaManzanas,
    DateOnly? FechaSiembra);

public record CultivoDto(
    int Id,
    int FincaId,
    string Nombre,
    int TipoCultivoId,
    string TipoCultivo,
    string? Variedad,
    decimal? AreaManzanas,
    DateOnly? FechaSiembra,
    bool Activo);

public record TipoCultivoRequestDto(string Nombre);

public record TipoCultivoDto(int Id, string Nombre);

public static class FincaMapeos
{
    private static T ADto<T>(Farm finca) where T : FincaDto, new() => new()
    {
        Id = finca.Id,
        Nombre = finca.Nombre,
        Departamento = finca.Departamento,
        Municipio = finca.Municipio,
        Direccion = finca.Ubicacion,
        AreaManzanas = finca.AreaManzanas,
        Descripcion = finca.Descripcion,
        CantidadCultivos = finca.Cultivos.Count
    };

    public static FincaDto ADto(this Farm finca) => ADto<FincaDto>(finca);

    public static FincaDetalleDto ADetalleDto(this Farm finca) => ADto<FincaDetalleDto>(finca) with
    {
        Productor = new ReferenciaDto(
            finca.ProductorId,
            finca.Productor?.Usuario?.NombreCompleto ?? string.Empty),
        Cultivos = finca.Cultivos.OrderBy(c => c.Nombre).Select(c => c.ADto()).ToList()
    };

    public static CultivoDto ADto(this Crop cultivo) => new(
        cultivo.Id,
        cultivo.FincaId,
        cultivo.Nombre,
        cultivo.TipoCultivoId,
        cultivo.TipoCultivo?.Nombre ?? string.Empty,
        cultivo.Variedad,
        cultivo.AreaSembrada,
        cultivo.FechaSiembra,
        cultivo.Activo);

    public static ReferenciaDto AReferencia(this Crop cultivo) => new(cultivo.Id, cultivo.Nombre);

    public static ReferenciaDto AReferencia(this Farm finca) => new(finca.Id, finca.Nombre);

    public static TipoCultivoDto ADto(this CropType tipo) => new(tipo.Id, tipo.Nombre);
}
