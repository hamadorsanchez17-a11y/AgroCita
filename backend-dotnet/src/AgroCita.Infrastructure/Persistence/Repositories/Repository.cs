using System.Linq.Expressions;
using AgroCita.Application.Common.Interfaces;
using AgroCita.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AgroCita.Infrastructure.Persistence.Repositories;

public class Repository<T>(AgroCitaDbContext context) : IRepository<T> where T : class
{
    public Task<T?> FirstAsync(
        Expression<Func<T, bool>> filtro,
        string[]? incluir = null,
        bool sinFiltroEmpresa = false,
        CancellationToken ct = default) =>
        Consulta(incluir, sinFiltroEmpresa).FirstOrDefaultAsync(filtro, ct);

    public Task<List<T>> ListAsync(
        Expression<Func<T, bool>>? filtro = null,
        Func<IQueryable<T>, IQueryable<T>>? ordenar = null,
        int? saltar = null,
        int? tomar = null,
        string[]? incluir = null,
        bool sinFiltroEmpresa = false,
        CancellationToken ct = default)
    {
        var consulta = Consulta(incluir, sinFiltroEmpresa);

        if (filtro is not null)
        {
            consulta = consulta.Where(filtro);
        }

        if (ordenar is not null)
        {
            consulta = ordenar(consulta);
        }

        if (saltar is not null)
        {
            consulta = consulta.Skip(saltar.Value);
        }

        if (tomar is not null)
        {
            consulta = consulta.Take(tomar.Value);
        }

        return consulta.ToListAsync(ct);
    }

    public Task<int> CountAsync(
        Expression<Func<T, bool>>? filtro = null,
        bool sinFiltroEmpresa = false,
        CancellationToken ct = default)
    {
        var consulta = Consulta(null, sinFiltroEmpresa);

        return filtro is null ? consulta.CountAsync(ct) : consulta.CountAsync(filtro, ct);
    }

    public Task<bool> AnyAsync(
        Expression<Func<T, bool>> filtro,
        bool sinFiltroEmpresa = false,
        CancellationToken ct = default) =>
        Consulta(null, sinFiltroEmpresa).AnyAsync(filtro, ct);

    public void Add(T entidad) => context.Set<T>().Add(entidad);

    private IQueryable<T> Consulta(string[]? incluir, bool sinFiltroEmpresa)
    {
        IQueryable<T> consulta = context.Set<T>();

        if (sinFiltroEmpresa)
        {
            consulta = consulta.IgnoreQueryFilters();
        }

        foreach (var relacion in incluir ?? [])
        {
            consulta = consulta.Include(relacion);
        }

        return consulta;
    }
}

public class UnitOfWork(AgroCitaDbContext context) : IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken ct = default) =>
        context.SaveChangesAsync(ct);
}
