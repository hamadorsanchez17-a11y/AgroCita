using AgroCita.Application.Common.Exceptions;
using AgroCita.Application.Common.Interfaces;
using AgroCita.Application.Common.Models;
using AgroCita.Application.Common.Services;
using AgroCita.Domain.Constants;
using AgroCita.Domain.Entities;
using FluentValidation;

namespace AgroCita.Application.Features.Tecnicos;

public class TecnicosHandler(
    IRepository<Technician> tecnicos,
    IRepository<User> usuarios,
    IUnitOfWork unitOfWork,
    SuscripcionService suscripcion,
    IPasswordHashService passwordHashService,
    IValidator<CrearTecnicoDto> crearValidator,
    IValidator<ActualizarTecnicoDto> actualizarValidator,
    IValidator<CambiarEstadoUsuarioDto> estadoValidator)
{
    public async Task<IReadOnlyList<TecnicoDto>> ListarAsync(CancellationToken ct)
    {
        var lista = await tecnicos.ListAsync(
            ordenar: q => q.OrderBy(t => t.Usuario!.Nombre).ThenBy(t => t.Usuario!.Apellido),
            incluir: ["Usuario"],
            ct: ct);

        return lista.Select(t => t.ADto()).ToList();
    }

    public async Task<TecnicoDto> CrearAsync(CrearTecnicoDto dto, CancellationToken ct)
    {
        await crearValidator.ValidateAndThrowAsync(dto, ct);

        var email = dto.Email.Trim().ToLowerInvariant();

        if (await usuarios.AnyAsync(u => u.Email == email, sinFiltroEmpresa: true, ct))
        {
            throw new ConflictException("Ya existe una cuenta con ese correo electrónico.");
        }

        await suscripcion.ExigirCupoDeTecnicosAsync(ct);

        var tecnico = new Technician
        {
            Telefono = dto.Telefono,
            Especialidad = dto.Especialidad?.Trim(),
            Activo = true,
            Usuario = new User
            {
                RolId = Roles.TecnicoId,
                Nombre = dto.Nombre.Trim(),
                Apellido = dto.Apellido.Trim(),
                Email = email,
                PasswordHash = passwordHashService.HashPassword(dto.PasswordTemporal),
                Activo = true,
                FechaCreacion = Fechas.Ahora
            }
        };

        tecnicos.Add(tecnico);
        await unitOfWork.SaveChangesAsync(ct);

        return tecnico.ADto();
    }

    public async Task<TecnicoDto> ActualizarAsync(
        int id, ActualizarTecnicoDto dto, CancellationToken ct)
    {
        await actualizarValidator.ValidateAndThrowAsync(dto, ct);

        var tecnico = await BuscarAsync(id, ct);

        tecnico.Usuario!.Nombre = dto.Nombre.Trim();
        tecnico.Usuario.Apellido = dto.Apellido.Trim();
        tecnico.Telefono = dto.Telefono;
        tecnico.Especialidad = dto.Especialidad?.Trim();

        await unitOfWork.SaveChangesAsync(ct);

        return tecnico.ADto();
    }

    public async Task CambiarEstadoAsync(
        int id, CambiarEstadoUsuarioDto dto, CancellationToken ct)
    {
        await estadoValidator.ValidateAndThrowAsync(dto, ct);

        var tecnico = await BuscarAsync(id, ct);

        tecnico.Activo = dto.Activo!.Value;
        tecnico.Usuario!.Activo = dto.Activo.Value;

        await unitOfWork.SaveChangesAsync(ct);
    }

    private async Task<Technician> BuscarAsync(int id, CancellationToken ct) =>
        await tecnicos.FirstAsync(t => t.Id == id, ["Usuario"], ct: ct)
        ?? throw new NotFoundException("El técnico no existe.");
}
