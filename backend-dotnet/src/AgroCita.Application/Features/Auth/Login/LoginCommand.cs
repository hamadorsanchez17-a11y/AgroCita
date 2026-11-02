
namespace AgroCita.Application.Features.Auth.Login;

//Solicitud interna del inicio de sesión
public record LoginCommand(
    string Email,
    string Password
);
