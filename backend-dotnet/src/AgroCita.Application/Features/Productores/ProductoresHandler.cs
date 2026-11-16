using System.Linq.Expressions;
using AgroCita.Application.Common.Exceptions;
using AgroCita.Application.Common.Interfaces;
using AgroCita.Application.Common.Models;
using AgroCita.Application.Common.Services;
using AgroCita.Domain.Entities;
using FluentValidation;

namespace AgroCita.Application.Features.Productores;

public class ProductoresHandler(
    ActorActual actor,
    IRepository<Producer> productores,
    IUnitOfWork unitOfWork,
    IValidator<ActualizarProductorDto> validator)
{
    public async Task<ProductorDto> ObtenerMiPerfilAsync(CancellationToken ct) =>
        (await actor.ProductorAsync(ct)).ADto();

    public async Task<ProductorDto> ActualizarMiPerfilAsync(
        ActualizarProductorDto dto, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(dto, ct);

        var productor = await actor.ProductorAsync(ct);

        productor.Usuario!.Nombre = dto.Nombre.Trim();
        productor.Usuario.Apellido = dto.Apellido.Trim();
        productor.Telefono = dto.Telefono;
        productor.Direccion = dto.Direccion?.Trim();

        await unitOfWork.SaveChangesAsync(ct);

        return productor.ADto();
    }

    public async Task<PaginadoDto<ProductorResumenDto>> ListarAsync(
        string? buscar, int? pagina, int? tamano, CancellationToken ct)
    {
        var (numeroPagina, tamanoPagina, saltar) = Paginacion.Normalizar(pagina, tamano);
        var texto = buscar?.Trim().ToLower();

        Expression<Func<Producer, bool>> filtro = p => true;

        if (!string.IsNullOrEmpty(texto))
        {
            filtro = p =>
                p.Usuario!.Nombre.ToLower().Contains(texto) ||
                p.Usuario.Apellido.ToLower().Contains(texto) ||
                p.Dni.Contains(texto);
        }

        var total = await productores.CountAsync(filtro, ct: ct);

        var lista = await productores.ListAsync(
            filtro,
            q => q.OrderBy(p => p.Usuario!.Nombre).ThenBy(p => p.Usuario!.Apellido),
            saltar,
            tamanoPagina,
            ["Usuario", "Fincas"],
            ct: ct);

        return new PaginadoDto<ProductorResumenDto>(
            lista.Select(p => p.AResumenDto()).ToList(),
            total,
            numeroPagina,
            tamanoPagina);
    }

    public async Task<ProductorDetalleDto> ObtenerAsync(int id, CancellationToken ct)
    {
        var productor = await productores.FirstAsync(
                p => p.Id == id, ["Usuario", "Fincas.Cultivos"], ct: ct)
            ?? throw new NotFoundException("El productor no existe.");

        return productor.ADetalleDto();
    }
}
