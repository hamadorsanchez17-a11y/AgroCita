namespace AgroCita.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(
        int userId,
        string email,
        string role,
        int? companyId
    );
}