using System.Linq.Expressions;

namespace AgroCita.Application.Common.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> FirstAsync(
        Expression<Func<T, bool>> filtro,
        string[]? incluir = null,
        bool sinFiltroEmpresa = false,
        CancellationToken ct = default);

    Task<List<T>> ListAsync(
        Expression<Func<T, bool>>? filtro = null,
        Func<IQueryable<T>, IQueryable<T>>? ordenar = null,
        int? saltar = null,
        int? tomar = null,
        string[]? incluir = null,
        bool sinFiltroEmpresa = false,
        CancellationToken ct = default);

    Task<int> CountAsync(
        Expression<Func<T, bool>>? filtro = null,
        bool sinFiltroEmpresa = false,
        CancellationToken ct = default);

    Task<bool> AnyAsync(
        Expression<Func<T, bool>> filtro,
        bool sinFiltroEmpresa = false,
        CancellationToken ct = default);

    void Add(T entidad);
}

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken ct = default);
}
