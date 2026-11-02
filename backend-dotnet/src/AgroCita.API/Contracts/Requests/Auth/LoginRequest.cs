
using System.ComponentModel.DataAnnotations;

namespace AgroCita.API.Contracts.Requests.Auth;

//Representación de los datos que enviará el usuario
public class LoginRequest
{
    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    public string Password { get; set; } = string.Empty;
}
