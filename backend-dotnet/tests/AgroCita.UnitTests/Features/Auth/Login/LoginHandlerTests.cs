
using AgroCita.Application.Common.Interfaces;
using AgroCita.Application.Features.Auth.Login;
using FluentValidation;

namespace AgroCita.UnitTests.Features.Auth.Login;

/*
Pruebas unitarias de LoginHandler para verificar
que el proceso de autenticación funcione correctamente
antes de realizar la conexión a PostgreSQL
*/
public class LoginHandlerTests
{
    private readonly FakeAuthenticationRepository _repository;
    private readonly FakePasswordHashService _passwordHashService;
    private readonly FakeJwtTokenGenerator _jwtTokenGenerator;
    private readonly LoginHandler _handler;

   public LoginHandlerTests()
{
    _repository = new FakeAuthenticationRepository();
    _passwordHashService = new FakePasswordHashService();
    _jwtTokenGenerator = new FakeJwtTokenGenerator();

    IValidator<LoginCommand> validator = new LoginCommandValidator();

    _handler = new LoginHandler(
        _repository,
        _passwordHashService,
        _jwtTokenGenerator,
        validator
    );
}

    [Fact]
    public async Task HandleAsync_ShouldReturnToken_WhenCredentialsAreCorrect()
    {
        // Arrange
        _repository.User = new AuthenticationUser
        {
            Id = 1,
            Email = "productor@agrocita.com",
            PasswordHash = "hash_de_prueba",
            Role = "PRODUCTOR",
            CompanyId = 10,
            IsActive = true
        };

        _passwordHashService.PasswordIsValid = true;
        _jwtTokenGenerator.Token = "token_jwt_de_prueba";

        var command = new LoginCommand(
            "productor@agrocita.com",
            "Password123"
        );

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("token_jwt_de_prueba", result.AccessToken);

        Assert.True(_passwordHashService.VerifyWasCalled);
        Assert.True(_jwtTokenGenerator.GenerateWasCalled);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        _repository.User = null;

        var command = new LoginCommand(
            "inexistente@agrocita.com",
            "Password123"
        );

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.Null(result);

        Assert.False(_passwordHashService.VerifyWasCalled);
        Assert.False(_jwtTokenGenerator.GenerateWasCalled);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnNull_WhenUserIsInactive()
    {
        // Arrange
        _repository.User = new AuthenticationUser
        {
            Id = 1,
            Email = "productor@agrocita.com",
            PasswordHash = "hash_de_prueba",
            Role = "PRODUCTOR",
            CompanyId = 10,
            IsActive = false
        };

        var command = new LoginCommand(
            "productor@agrocita.com",
            "Password123"
        );

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.Null(result);

        Assert.False(_passwordHashService.VerifyWasCalled);
        Assert.False(_jwtTokenGenerator.GenerateWasCalled);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnNull_WhenPasswordIsIncorrect()
    {
        // Arrange
        _repository.User = new AuthenticationUser
        {
            Id = 1,
            Email = "productor@agrocita.com",
            PasswordHash = "hash_de_prueba",
            Role = "PRODUCTOR",
            CompanyId = 10,
            IsActive = true
        };

        _passwordHashService.PasswordIsValid = false;

        var command = new LoginCommand(
            "productor@agrocita.com",
            "PasswordIncorrecta"
        );

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.Null(result);

        Assert.True(_passwordHashService.VerifyWasCalled);
        Assert.False(_jwtTokenGenerator.GenerateWasCalled);
    }

    [Fact]
    public async Task HandleAsync_ShouldTrimEmail_BeforeSearchingUser()
    {
        // Arrange
        _repository.User = new AuthenticationUser
        {
            Id = 1,
            Email = "productor@agrocita.com",
            PasswordHash = "hash_de_prueba",
            Role = "PRODUCTOR",
            CompanyId = 10,
            IsActive = true
        };

        _passwordHashService.PasswordIsValid = true;

        var command = new LoginCommand(
            "  productor@agrocita.com  ",
            "Password123"
        );

        // Act
        await _handler.HandleAsync(command);

        // Assert
        Assert.Equal(
            "productor@agrocita.com",
            _repository.LastSearchedEmail
        );
    }

    [Fact]
    
public async Task HandleAsync_ShouldThrowValidationException_WhenCredentialsAreEmpty()
{
    // Arrange
    var command = new LoginCommand("", "");

    // Act
    var exception = await Assert.ThrowsAsync<ValidationException>(
        async () => await _handler.HandleAsync(command)
    );

    // Assert
    Assert.NotEmpty(exception.Errors);
}

    // Objetos simulados para las dependencias del LoginHandler.

    private class FakeAuthenticationRepository : IAuthenticationRepository
    {
        public AuthenticationUser? User { get; set; }

        public string? LastSearchedEmail { get; private set; }

        public bool GetByEmailWasCalled { get; private set; }

        public Task<AuthenticationUser?> GetByEmailAsync(
            string email,
            CancellationToken cancellationToken = default)
        {
            GetByEmailWasCalled = true;
            LastSearchedEmail = email;

            return Task.FromResult(User);
        }
    }

    private class FakePasswordHashService : IPasswordHashService
    {
        public bool PasswordIsValid { get; set; }

        public bool VerifyWasCalled { get; private set; }

        public string HashPassword(string password)
        {
            return $"HASH_{password}";
        }

        public bool VerifyPassword(
            string hashedPassword,
            string providedPassword)
        {
            VerifyWasCalled = true;

            return PasswordIsValid;
        }
    }

    private class FakeJwtTokenGenerator : IJwtTokenGenerator
    {
        public string Token { get; set; } = "token_de_prueba";

        public bool GenerateWasCalled { get; private set; }

        public string GenerateToken(
            int userId,
            string email,
            string role,
            int? companyId)
        {
            GenerateWasCalled = true;

            return Token;
        }
    }
}
