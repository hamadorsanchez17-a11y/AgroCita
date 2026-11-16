using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AgroCita.Application.Common.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AgroCita.Infrastructure.Services.Security;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }

    public int ExpiresInSeconds => _jwtSettings.ExpirationMinutes * 60;

    public string GenerateToken(
        int userId,
        string email,
        string role,
        int? companyId)
    {
        var claims = new List<Claim>
        {
            new(JwtClaims.UsuarioId, userId.ToString()),
            new(JwtClaims.Email, email),
            new(JwtClaims.Rol, role)
        };

        if (companyId.HasValue)
        {
            claims.Add(new Claim(JwtClaims.EmpresaId, companyId.Value.ToString()));
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.UtcNow.AddMinutes(
            _jwtSettings.ExpirationMinutes);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
