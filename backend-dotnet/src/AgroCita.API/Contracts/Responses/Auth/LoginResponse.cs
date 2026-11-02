
namespace AgroCita.API.Contracts.Responses.Auth;

//Contrato que representa la respuesta de un inicio de sesión exitoso
public class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;

    public string TokenType { get; set; } = "Bearer";

    public int ExpiresInSeconds { get; set; }
}
