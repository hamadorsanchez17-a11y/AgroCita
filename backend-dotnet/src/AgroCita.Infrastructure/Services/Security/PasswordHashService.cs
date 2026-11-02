using AgroCita.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AgroCita.Infrastructure.Services.Security;

public class PasswordHashService : IPasswordHashService
{
    private readonly PasswordHasher<object> _passwordHasher;

    public PasswordHashService()
    {
        _passwordHasher = new PasswordHasher<object>();
    }

    public string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException(
                "La contraseña no puede estar vacía.",
                nameof(password));
        }

        return _passwordHasher.HashPassword(
            new object(),
            password);
    }

    public bool VerifyPassword(
        string hashedPassword,
        string providedPassword)
    {
        if (string.IsNullOrWhiteSpace(hashedPassword) ||
            string.IsNullOrEmpty(providedPassword))
        {
            return false;
        }

        var result = _passwordHasher.VerifyHashedPassword(
            new object(),
            hashedPassword,
            providedPassword);

        return result != PasswordVerificationResult.Failed;
    }
}