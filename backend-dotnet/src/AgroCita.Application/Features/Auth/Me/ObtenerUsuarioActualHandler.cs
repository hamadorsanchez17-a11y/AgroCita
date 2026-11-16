using AgroCita.Application.Common.Exceptions;
using AgroCita.Application.Common.Interfaces;
using AgroCita.Domain.Entities;

namespace AgroCita.Application.Features.Auth.Me;

public class ObtenerUsuarioActualHandler(
    ICurrentUserService usuarioActual,
    IRepository<User> usuarios)
{
    public async Task<UsuarioActualDto> HandleAsync(CancellationToken ct = default)
    {
        var usuarioId = usuarioActual.UsuarioId;

        var usuario = await usuarios.FirstAsync(
                u => u.Id == usuarioId && u.Activo,
                ["Rol", "Empresa"],
                sinFiltroEmpresa: true,
                ct)
            ?? throw new NotFoundException("El usuario no existe.");

        return new UsuarioActualDto(
            usuario.Id,
            usuario.NombreCompleto,
            usuario.Email,
            usuario.Rol!.Nombre,
            usuario.EmpresaId,
            usuario.Empresa?.Nombre);
    }
}
