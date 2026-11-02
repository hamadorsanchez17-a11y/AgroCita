
namespace AgroCita.Application.Features.Auth.Login;

public class AuthenticationUser
{
    public int Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public int? CompanyId { get; set; }

    public bool IsActive { get; set; }
}
