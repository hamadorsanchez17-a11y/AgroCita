using AgroCita.Application.Common.Interfaces;
using FluentValidation;

namespace AgroCita.Application.Features.Auth.Login;

public class LoginHandler
{
    private readonly IAuthenticationRepository _authenticationRepository;
    private readonly IPasswordHashService _passwordHashService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IValidator<LoginCommand> _validator;

    public LoginHandler(
        IAuthenticationRepository authenticationRepository,
        IPasswordHashService passwordHashService,
        IJwtTokenGenerator jwtTokenGenerator,
        IValidator<LoginCommand> validator)
    {
        _authenticationRepository = authenticationRepository;
        _passwordHashService = passwordHashService;
        _jwtTokenGenerator = jwtTokenGenerator;
        _validator = validator;
    }

    public async Task<LoginResult?> HandleAsync(
        LoginCommand command,
        CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(
            command,
            cancellationToken);

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