namespace AgroCita.Application.Common.Models;

public record ReferenciaDto(int Id, string Nombre);

public record PaginadoDto<T>(IReadOnlyList<T> Items, int Total, int Pagina, int Tamano);

public static class Paginacion
{
    public const int TamanoMaximo = 100;

    public static (int Pagina, int Tamano, int Saltar) Normalizar(int? pagina, int? tamano)
    {
        var p = pagina is > 0 ? pagina.Value : 1;
        var t = tamano is > 0 ? Math.Min(tamano.Value, TamanoMaximo) : 20;

        return (p, t, (p - 1) * t);
    }
}

public static class Fechas
{
    private static readonly TimeSpan DesfaseHonduras = TimeSpan.FromHours(-6);

    public static DateTime Ahora => DateTime.UtcNow;

    public static DateOnly Hoy => DateOnly.FromDateTime(DateTime.UtcNow + DesfaseHonduras);

    public static DateTime AUtc(this DateTime fecha) => fecha.Kind switch
    {
        DateTimeKind.Utc => fecha,
        DateTimeKind.Local => fecha.ToUniversalTime(),
        _ => DateTime.SpecifyKind(fecha, DateTimeKind.Utc)
    };

    public static DateTime InicioUtc(this DateOnly fecha) =>
        fecha.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
}
