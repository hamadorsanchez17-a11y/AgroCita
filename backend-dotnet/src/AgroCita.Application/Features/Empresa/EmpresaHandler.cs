using System.Security.Cryptography;
using AgroCita.Application.Common.Exceptions;
using AgroCita.Application.Common.Interfaces;
using AgroCita.Application.Common.Services;
using AgroCita.Domain.Entities;

namespace AgroCita.Application.Features.Empresa;

public class EmpresaHandler(
    IRepository<Company> empresas,
    IUnitOfWork unitOfWork,
    SuscripcionService suscripcion)
{
    private const string Caracteres = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

    public async Task<EmpresaDto> ObtenerAsync(CancellationToken ct) =>
        (await ActualAsync(ct)).ADto();

    public async Task<EmpresaDto> RegenerarCodigoAsync(CancellationToken ct)
    {
        var empresa = await ActualAsync(ct);

        empresa.CodigoInvitacion = await GenerarCodigoAsync(empresas, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return empresa.ADto();
    }

    public async Task<SuscripcionDto> ObtenerSuscripcionAsync(CancellationToken ct)
    {
        var vigente = await suscripcion.VigenteAsync(ct)
            ?? throw new NotFoundException("La organización no tiene una suscripción asignada.");

        return vigente.ADto();
    }

    public static async Task<string> GenerarCodigoAsync(
        IRepository<Company> empresas, CancellationToken ct)
    {
        while (true)
        {
            var codigo = RandomNumberGenerator.GetString(Caracteres, 12);

            if (!await empresas.AnyAsync(
                    e => e.CodigoInvitacion == codigo, sinFiltroEmpresa: true, ct))
            {
                return codigo;
            }
        }
    }

    private async Task<Company> ActualAsync(CancellationToken ct) =>
        await empresas.FirstAsync(e => true, ct: ct)
        ?? throw new NotFoundException("La organización no existe.");
}
