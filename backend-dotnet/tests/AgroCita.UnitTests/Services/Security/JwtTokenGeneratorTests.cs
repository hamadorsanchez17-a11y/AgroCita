
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AgroCita.Infrastructure.Services.Security;
using Microsoft.Extensions.Options;
using Xunit;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AgroCita.UnitTests.Services.Security;

public class JwtTokenGeneratorTests
{
    private readonly JwtTokenGenerator _generator;
    private readonly JwtSettings _settings;

    public JwtTokenGeneratorTests()
    {
            _settings = new JwtSettings
            {
                SecretKey = Convert.ToBase64String(
                    System.Security.Cryptography.RandomNumberGenerator.GetBytes(64)),
                Issuer = "AgroCita",
                Audience = "AgroCitaUsers",
                ExpirationMinutes = 60
            };

            _generator = new JwtTokenGenerator(
                Options.Create(_settings));
    }

    [Fact]
    public void GenerateToken_ShouldIncludeUserClaims()
    {
        // Arrange
        const int userId = 15;
        const string email = "usuario@agrocita.com";
        const string role = "PRODUCTOR";
        const int companyId = 3;

        // Act
        var tokenString = _generator.GenerateToken(
            userId, email, role, companyId);

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(tokenString);

        // Assert
        Assert.Equal(userId.ToString(),
            token.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

        Assert.Equal(email,
            token.Claims.First(c => c.Type == ClaimTypes.Email).Value);

        Assert.Equal(role,
            token.Claims.First(c => c.Type == ClaimTypes.Role).Value);

        Assert.Equal(companyId.ToString(),
            token.Claims.First(c => c.Type == "company_id").Value);
    }

    [Fact]
    public void GenerateToken_WithoutCompany_ShouldNotIncludeCompanyId()
    {
        // Act
        var tokenString = _generator.GenerateToken(
            15,
            "admin@agrocita.com",
            "ADMINISTRADOR",
            null);

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(tokenString);

        // Assert
        Assert.DoesNotContain(
            token.Claims,
            claim => claim.Type == "company_id");
    }

    [Fact]
    public void GenerateToken_ShouldHaveCorrectIssuerAndAudience()
    {
        // Act
        var tokenString = _generator.GenerateToken(
            15,
            "usuario@agrocita.com",
            "PRODUCTOR",
            3);

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(tokenString);

        // Assert
        Assert.Equal("AgroCita", token.Issuer);

        Assert.Contains(
            "AgroCitaUsers",
            token.Audiences);
    }

    [Fact]
    public void GenerateToken_ShouldExpireInConfiguredTime()
    {
        // Arrange
        var beforeGeneration = DateTime.UtcNow;

        // Act
        var tokenString = _generator.GenerateToken(
            15,
            "usuario@agrocita.com",
            "PRODUCTOR",
            3);

        var afterGeneration = DateTime.UtcNow;

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(tokenString);

        // Assert
        var expectedMinimum = beforeGeneration.AddMinutes(60);
        var expectedMaximum = afterGeneration.AddMinutes(60);

        Assert.InRange(
            token.ValidTo,
            expectedMinimum.AddSeconds(-1),
            expectedMaximum.AddSeconds(1));
    }

    //Verifica la firma criptográfica, emisor, audiencia y vigencia del token
    [Fact]
    public void GenerateToken_ShouldHaveValidSignature()
    {
        // Arrange
        var tokenString = _generator.GenerateToken(
            15,
            "usuario@agrocita.com",
            "PRODUCTOR",
            3);

        var handler = new JwtSecurityTokenHandler();

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _settings.Issuer,

            ValidateAudience = true,
            ValidAudience = _settings.Audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_settings.SecretKey)),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        // Act
        var principal = handler.ValidateToken(
            tokenString,
            validationParameters,
            out var validatedToken);

        // Assert
        Assert.NotNull(principal);
        Assert.IsType<JwtSecurityToken>(validatedToken);
        Assert.Equal(
            "15",
            principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    }
}
