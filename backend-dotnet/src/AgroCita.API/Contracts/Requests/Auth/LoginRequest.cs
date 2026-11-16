
namespace AgroCita.API.Contracts.Requests.Auth;

//Representación de los datos que enviará el usuario
public class LoginRequest
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
