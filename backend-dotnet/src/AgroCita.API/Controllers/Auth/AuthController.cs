
using AgroCita.API.Contracts.Requests.Auth;
using AgroCita.API.Contracts.Responses.Auth;
using AgroCita.Application.Features.Auth.Login;
using AgroCita.Infrastructure.Services.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AgroCita.API.Controllers.Auth;

/*Controlador para el endpoint REST
POST /api/auth/login*/

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly LoginHandler _loginHandler;
    private readonly JwtSettings _jwtSettings;

    public AuthController(
        LoginHandler loginHandler,
        IOptions<JwtSettings> jwtOptions)
    {
        _loginHandler = loginHandler;
        _jwtSettings = jwtOptions.Value;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(
            request.Email,
            request.Password
        );

        var result = await _loginHandler.HandleAsync(
            command,
            cancellationToken
        );

        if (result is null)
        {
            return Unauthorized(new
            {
                message = "Correo electrónico o contraseña incorrectos."
            });
        }

        var response = new LoginResponse
        {
            AccessToken = result.AccessToken,
            TokenType = "Bearer",
            ExpiresInSeconds = _jwtSettings.ExpirationMinutes * 60
        };

        return Ok(response);
    }
}
