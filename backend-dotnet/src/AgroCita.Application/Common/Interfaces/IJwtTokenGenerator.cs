namespace AgroCita.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    int ExpiresInSeconds { get; }

    string GenerateToken(
        int userId,
        string email,
        string role,
        int? companyId
    );
}
