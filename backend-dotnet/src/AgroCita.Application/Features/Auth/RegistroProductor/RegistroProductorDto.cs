namespace AgroCita.Application.Features.Auth.RegistroProductor;

public record RegistroProductorDto(
    string CodigoInvitacion,
    string Nombre,
    string Apellido,
    string Dni,
    string Telefono,
    string Email,
    string Password);
