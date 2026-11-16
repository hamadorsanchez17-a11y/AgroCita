using AgroCita.Application.Common.Exceptions;
using AgroCita.Application.Common.Interfaces;
using AgroCita.Application.Common.Services;
using AgroCita.Domain.Entities;
using FluentValidation;

namespace AgroCita.Application.Features.Fincas;

public class FincasHandler(
    ActorActual actor,
    IRepository<Farm> fincas,
    IUnitOfWork unitOfWork,
    IValidator<FincaRequestDto> validator)
{
    public async Task<IReadOnlyList<FincaDto>> ListarMiasAsync(CancellationToken ct)
    {
        var productorId = (await actor.ProductorAsync(ct)).Id;

        var lista = await fincas.ListAsync(
            f => f.ProductorId == productorId,
            q => q.OrderBy(f => f.Nombre),
            incluir: ["Cultivos"],
            ct: ct);

        return lista.Select(f => f.ADto()).ToList();
    }

    public async Task<FincaDetalleDto> ObtenerAsync(int id, CancellationToken ct)
    {
        var finca = await actor.FincaAsync(
            id, ct, "Productor.Usuario", "Cultivos.TipoCultivo");

        return finca.ADetalleDto();
    }

    public async Task<FincaDto> CrearAsync(FincaRequestDto dto, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(dto, ct);

        var productorId = (await actor.ProductorAsync(ct)).Id;
        await ExigirNombreLibreAsync(productorId, dto.Nombre.Trim(), 0, ct);

        var finca = new Farm { ProductorId = productorId };
        Aplicar(finca, dto);

        fincas.Add(finca);
        await unitOfWork.SaveChangesAsync(ct);

        return finca.ADto();
    }

    public async Task<FincaDto> ActualizarAsync(int id, FincaRequestDto dto, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(dto, ct);

        var finca = await actor.FincaAsync(id, ct, "Cultivos");
        await ExigirNombreLibreAsync(finca.ProductorId, dto.Nombre.Trim(), finca.Id, ct);

        Aplicar(finca, dto);
        await unitOfWork.SaveChangesAsync(ct);

        return finca.ADto();
    }

    private static void Aplicar(Farm finca, FincaRequestDto dto)
    {
        finca.Nombre = dto.Nombre.Trim();
        finca.Departamento = dto.Departamento.Trim();
        finca.Municipio = dto.Municipio.Trim();
        finca.Ubicacion = dto.Direccion.Trim();
        finca.AreaManzanas = dto.AreaManzanas;
        finca.Descripcion = dto.Descripcion?.Trim();
    }

    private async Task ExigirNombreLibreAsync(
        int productorId, string nombre, int fincaId, CancellationToken ct)
    {
        if (await fincas.AnyAsync(
                f => f.ProductorId == productorId && f.Nombre == nombre && f.Id != fincaId,
                ct: ct))
        {
            throw new ConflictException("Ya tiene una finca registrada con ese nombre.");
        }
    }
}
