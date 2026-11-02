
using AgroCita.Application.Common.Interfaces;

namespace AgroCita.Application.Features.Auth.Login;

//Coordina el proceso de autenticación
public class LoginHandler
{
    private readonly IAuthenticationRepository _authenticationRepository;
    private readonly IPasswordHashService _passwordHashService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginHandler(
        IAuthenticationRepository authenticationRepository,
        IPasswordHashService passwordHashService,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _authenticationRepository = authenticationRepository;
        _passwordHashService = passwordHashService;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<LoginResult?> HandleAsync(
        LoginCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Email) ||
            string.IsNullOrEmpty(command.Password))
        {
            return null;
        }

        var email = command.Email.Trim();

        var user = await _authenticationRepository.GetByEmailAsync(
            email,
            cancellationToken);

        if (user is null || !user.IsActive)
        {
            return null;
        }

        var passwordIsValid = _passwordHashService.VerifyPassword(
            user.PasswordHash,
            command.Password);

        if (!passwordIsValid)
        {
            return null;
        }

        var token = _jwtTokenGenerator.GenerateToken(
            user.Id,
            user.Email,
            user.Role,
            user.CompanyId);

        return new LoginResult(token);
    }
}
