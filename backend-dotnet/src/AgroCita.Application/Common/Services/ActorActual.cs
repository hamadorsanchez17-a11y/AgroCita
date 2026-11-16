using AgroCita.Application.Common.Exceptions;
using AgroCita.Application.Common.Interfaces;
using AgroCita.Domain.Constants;
using AgroCita.Domain.Entities;

namespace AgroCita.Application.Common.Services;

public class ActorActual(
    ICurrentUserService usuarioActual,
    IRepository<Producer> productores,
    IRepository<Technician> tecnicos,
    IRepository<Farm> fincas,
    IRepository<Crop> cultivos,
    IRepository<Visit> visitas)
{
    private Producer? _productor;
    private Technician? _tecnico;

    public int UsuarioId =>
        usuarioActual.UsuarioId ?? throw new ForbiddenException("La sesión no es válida.");

    public int EmpresaId =>
        usuarioActual.EmpresaId ?? throw new ForbiddenException("El usuario no pertenece a una organización.");

    public bool EsProductor => usuarioActual.Rol == Roles.Productor;

    public bool EsTecnico => usuarioActual.Rol == Roles.Tecnico;

    public async Task<Producer> ProductorAsync(CancellationToken ct)
    {
        var usuarioId = UsuarioId;

        return _productor ??= await productores.FirstAsync(
                p => p.UsuarioId == usuarioId, ["Usuario"], ct: ct)
            ?? throw new ForbiddenException("El usuario no tiene un perfil de productor.");
    }

    public async Task<Technician> TecnicoAsync(CancellationToken ct)
    {
        var usuarioId = UsuarioId;

        return _tecnico ??= await tecnicos.FirstAsync(
                t => t.UsuarioId == usuarioId, ["Usuario"], ct: ct)
            ?? throw new ForbiddenException("El usuario no tiene un perfil de técnico.");
    }

    public async Task<Farm> FincaAsync(int id, CancellationToken ct, params string[] incluir)
    {
        var finca = await fincas.FirstAsync(f => f.Id == id, incluir, ct: ct)
            ?? throw new NotFoundException("La finca no existe.");

        if (EsProductor && finca.ProductorId != (await ProductorAsync(ct)).Id)
        {
            throw new NotFoundException("La finca no existe.");
        }

        return finca;
    }

    public async Task<Crop> CultivoAsync(int id, CancellationToken ct)
    {
        var cultivo = await cultivos.FirstAsync(
                c => c.Id == id, ["Finca", "TipoCultivo"], ct: ct)
            ?? throw new NotFoundException("El cultivo no existe.");

        if (EsProductor && cultivo.Finca!.ProductorId != (await ProductorAsync(ct)).Id)
        {
            throw new NotFoundException("El cultivo no existe.");
        }

        return cultivo;
    }

    public async Task<Visit> VisitaAsync(
        int id,
        bool soloTecnicoAsignado,
        CancellationToken ct,
        params string[] incluir)
    {
        string[] relaciones = ["Solicitud.Finca", .. incluir];

        var visita = await visitas.FirstAsync(v => v.Id == id, relaciones, ct: ct)
            ?? throw new NotFoundException("La visita no existe.");

        if (EsProductor &&
            visita.Solicitud!.Finca!.ProductorId != (await ProductorAsync(ct)).Id)
        {
            throw new NotFoundException("La visita no existe.");
        }

        if (EsTecnico && soloTecnicoAsignado &&
            visita.TecnicoId != (await TecnicoAsync(ct)).Id)
        {
            throw new NotFoundException("La visita no existe.");
        }

        return visita;
    }
}
