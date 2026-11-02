
namespace AgroCita.Application.Features.Auth.Login;

/*
Establece cómo la capa de Aplicación
solicitará los datos del usuario
*/
public interface IAuthenticationRepository
{
    Task<AuthenticationUser?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default
    );
}
