
using AgroCita.Infrastructure.Services.Security;

namespace AgroCita.UnitTests.Services.Security;

public class PasswordHashServiceTests
{
    private readonly PasswordHashService _passwordHashService;

    public PasswordHashServiceTests()
    {
        _passwordHashService = new PasswordHashService();
    }

    [Fact]
    public void HashPassword_ShouldReturnHashedPassword()
    {
        // Arrange
        var password = "AgroCita_Test_123";

        // Act
        var hashedPassword =
            _passwordHashService.HashPassword(password);

        // Assert
        Assert.NotNull(hashedPassword);
        Assert.NotEmpty(hashedPassword);
        Assert.NotEqual(password, hashedPassword);
    }

    [Fact]
    public void HashPassword_ShouldGenerateDifferentHashesForSamePassword()
    {
        // Arrange
        var password = "AgroCita_Test_123";

        // Act
        var firstHash = _passwordHashService.HashPassword(password);
        var secondHash = _passwordHashService.HashPassword(password);

        // Assert
        Assert.NotEqual(firstHash, secondHash);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnTrue_WhenPasswordIsCorrect()
    {
        // Arrange
        var password = "AgroCita_Test_123";
        var hashedPassword =
            _passwordHashService.HashPassword(password);

        // Act
        var result = _passwordHashService.VerifyPassword(
            hashedPassword,
            password);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnFalse_WhenPasswordIsIncorrect()
    {
        // Arrange
        var password = "AgroCita_Test_123";
        var incorrectPassword = "Password_Incorrecta";

        var hashedPassword =
            _passwordHashService.HashPassword(password);

        // Act
        var result = _passwordHashService.VerifyPassword(
            hashedPassword,
            incorrectPassword);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void HashPassword_ShouldThrow_WhenPasswordIsEmpty(string password)
    {
        // Act
        var exception = Assert.Throws<ArgumentException>(
            () => _passwordHashService.HashPassword(password));

        // Assert
        Assert.Equal("password", exception.ParamName);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnFalse_WhenHashIsEmpty()
    {
        // Arrange
        var password = "AgroCita_Test_123";

        // Act
        var result = _passwordHashService.VerifyPassword(
            string.Empty,
            password);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnFalse_WhenProvidedPasswordIsEmpty()
    {
        // Arrange
        var password = "AgroCita_Test_123";
        var hashedPassword =
            _passwordHashService.HashPassword(password);

        // Act
        var result = _passwordHashService.VerifyPassword(
            hashedPassword,
            string.Empty);

        // Assert
        Assert.False(result);
    }
}
