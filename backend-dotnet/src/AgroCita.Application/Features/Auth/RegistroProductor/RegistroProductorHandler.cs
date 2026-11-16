using AgroCita.Application.Common.Exceptions;
using AgroCita.Application.Common.Interfaces;
using AgroCita.Application.Common.Models;
using AgroCita.Application.Common.Services;
using AgroCita.Domain.Constants;
using AgroCita.Domain.Entities;
using FluentValidation;

namespace AgroCita.Application.Features.Auth.RegistroProductor;

public class RegistroProductorHandler(
    IRepository<Company> empresas,
    IRepository<User> usuarios,
    IRepository<Producer> productores,
    IUnitOfWork unitOfWork,
    SuscripcionService suscripcion,
    IPasswordHashService passwordHashService,
    IJwtTokenGenerator jwtTokenGenerator,
    IValidator<RegistroProductorDto> validator)
{
    public async Task<AuthResponseDto> HandleAsync(
        RegistroProductorDto dto,
        CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(dto, ct);

        var codigo = dto.CodigoInvitacion.Trim().ToUpperInvariant();
        var email = dto.Email.Trim().ToLowerInvariant();

        var empresa = await empresas.FirstAsync(
            e => e.CodigoInvitacion == codigo,
            ["Suscripciones.Plan"],
            sinFiltroEmpresa: true,
            ct);

        if (empresa is null || !empresa.Activa)
        {
            throw Errores.Campo(
                nameof(dto.CodigoInvitacion),
                "El código de invitación no es válido.");
        }

        if (empresa.SuscripcionVigente()?.PermiteOperar(Fechas.Hoy) != true)
        {
            throw new ForbiddenException(
                "La suscripción de la organización no está activa.");
        }

        if (await usuarios.AnyAsync(u => u.Email == email, sinFiltroEmpresa: true, ct))
        {
            throw new ConflictException("Ya existe una cuenta con ese correo electrónico.");
        }

        var empresaId = empresa.Id;
        var dni = dto.Dni;

        if (await productores.AnyAsync(
                p => p.EmpresaId == empresaId && p.Dni == dni,
                sinFiltroEmpresa: true,
                ct))
        {
            throw new ConflictException("Ya existe un productor con ese DNI en la organización.");
        }

        await suscripcion.ExigirCupoDeProductoresAsync(empresa, ct);

        var usuario = new User
        {
            EmpresaId = empresaId,
            RolId = Roles.ProductorId,
            Nombre = dto.Nombre.Trim(),
            Apellido = dto.Apellido.Trim(),
            Email = email,
            PasswordHash = passwordHashService.HashPassword(dto.Password),
            Activo = true,
            FechaCreacion = Fechas.Ahora
        };

        productores.Add(new Producer
        {
            EmpresaId = empresaId,
            Dni = dni,
            Telefono = dto.Telefono,
            Usuario = usuario
        });

        await unitOfWork.SaveChangesAsync(ct);

        var token = jwtTokenGenerator.GenerateToken(
            usuario.Id, usuario.Email, Roles.Productor, empresaId);

        return new AuthResponseDto(
            token,
            jwtTokenGenerator.ExpiresInSeconds,
            new UsuarioActualDto(
                usuario.Id,
                usuario.NombreCompleto,
                usuario.Email,
                Roles.Productor,
                empresaId,
                empresa.Nombre));
    }
}
