using AgroCita.Application.Common.Exceptions;
using AgroCita.Application.Common.Interfaces;
using AgroCita.Application.Common.Services;
using AgroCita.Domain.Entities;
using FluentValidation;

namespace AgroCita.Application.Features.Fincas;

public class CultivosHandler(
    ActorActual actor,
    IRepository<Crop> cultivos,
    IRepository<CropType> tipos,
    IUnitOfWork unitOfWork,
    IValidator<CultivoRequestDto> cultivoValidator,
    IValidator<TipoCultivoRequestDto> tipoValidator)
{
    public async Task<IReadOnlyList<CultivoDto>> ListarPorFincaAsync(int fincaId, CancellationToken ct)
    {
        await actor.FincaAsync(fincaId, ct);

        var lista = await cultivos.ListAsync(
            c => c.FincaId == fincaId,
            q => q.OrderBy(c => c.Nombre),
            incluir: ["TipoCultivo"],
            ct: ct);

        return lista.Select(c => c.ADto()).ToList();
    }

    public async Task<CultivoDto> CrearAsync(int fincaId, CultivoRequestDto dto, CancellationToken ct)
    {
        await cultivoValidator.ValidateAndThrowAsync(dto, ct);

        var finca = await actor.FincaAsync(fincaId, ct);
        var tipo = await TipoAsync(dto.TipoCultivoId, ct);

        var cultivo = new Crop { FincaId = finca.Id, Activo = true };
        Aplicar(cultivo, dto, tipo);

        cultivos.Add(cultivo);
        await unitOfWork.SaveChangesAsync(ct);

        return cultivo.ADto();
    }

    public async Task<CultivoDto> ActualizarAsync(int id, CultivoRequestDto dto, CancellationToken ct)
    {
        await cultivoValidator.ValidateAndThrowAsync(dto, ct);

        var cultivo = await actor.CultivoAsync(id, ct);
        var tipo = await TipoAsync(dto.TipoCultivoId, ct);

        Aplicar(cultivo, dto, tipo);
        await unitOfWork.SaveChangesAsync(ct);

        return cultivo.ADto();
    }

    public async Task<IReadOnlyList<TipoCultivoDto>> ListarTiposAsync(CancellationToken ct)
    {
        var lista = await tipos.ListAsync(ordenar: q => q.OrderBy(t => t.Nombre), ct: ct);

        return lista.Select(t => t.ADto()).ToList();
    }

    public async Task<TipoCultivoDto> CrearTipoAsync(TipoCultivoRequestDto dto, CancellationToken ct)
    {
        await tipoValidator.ValidateAndThrowAsync(dto, ct);

        var nombre = dto.Nombre.Trim();
        var nombreMinusculas = nombre.ToLower();

        if (await tipos.AnyAsync(t => t.Nombre.ToLower() == nombreMinusculas, ct: ct))
        {
            throw new ConflictException("Ya existe un tipo de cultivo con ese nombre.");
        }

        var tipo = new CropType { Nombre = nombre };

        tipos.Add(tipo);
        await unitOfWork.SaveChangesAsync(ct);

        return tipo.ADto();
    }

    private async Task<CropType> TipoAsync(int id, CancellationToken ct) =>
        await tipos.FirstAsync(t => t.Id == id, ct: ct)
        ?? throw Errores.Campo(
            nameof(CultivoRequestDto.TipoCultivoId),
            "El tipo de cultivo no existe en la organización.");

    private static void Aplicar(Crop cultivo, CultivoRequestDto dto, CropType tipo)
    {
        cultivo.TipoCultivoId = tipo.Id;
        cultivo.TipoCultivo = tipo;
        cultivo.Nombre = string.IsNullOrWhiteSpace(dto.Nombre) ? tipo.Nombre : dto.Nombre.Trim();
        cultivo.Variedad = dto.Variedad?.Trim();
        cultivo.AreaSembrada = dto.AreaManzanas;
        cultivo.FechaSiembra = dto.FechaSiembra;
    }
}
