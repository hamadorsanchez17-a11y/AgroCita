
namespace AgroCita.Application.Features.Auth.Login;

//Resultado exitoso del caso de uso
public record LoginResult(
    string AccessToken,
    int ExpiresInSeconds,
    UsuarioActualDto Usuario
);
