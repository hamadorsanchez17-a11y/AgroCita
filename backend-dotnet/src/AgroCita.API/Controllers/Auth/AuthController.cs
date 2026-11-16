
using AgroCita.API.Contracts.Requests.Auth;
using AgroCita.API.Filters;
using AgroCita.API.Middleware;
using AgroCita.Application.Features.Auth;
using AgroCita.Application.Features.Auth.Login;
using AgroCita.Application.Features.Auth.Me;
using AgroCita.Application.Features.Auth.RegistroProductor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroCita.API.Controllers.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly LoginHandler _loginHandler;
    private readonly RegistroProductorHandler _registroHandler;
    private readonly ObtenerUsuarioActualHandler _usuarioActualHandler;

    public AuthController(
        LoginHandler loginHandler,
        RegistroProductorHandler registroHandler,
        ObtenerUsuarioActualHandler usuarioActualHandler)
    {
        _loginHandler = loginHandler;
        _registroHandler = registroHandler;
        _usuarioActualHandler = usuarioActualHandler;
    }

    [HttpPost("registro-productor")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegistrarProductor(
        [FromBody] RegistroProductorDto request,
        CancellationToken cancellationToken)
    {
        var response = await _registroHandler.HandleAsync(request, cancellationToken);

        return Created("/api/auth/me", response);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
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
            return Unauthorized(new ErrorResponse(
                "Credenciales incorrectas",
                StatusCodes.Status401Unauthorized,
                "Correo electrónico o contraseña incorrectos."));
        }

        return Ok(new AuthResponseDto(
            result.AccessToken,
            result.ExpiresInSeconds,
            result.Usuario));
    }

    [HttpGet("me")]
    [Authorize]
    [SinValidarSuscripcion]
    [ProducesResponseType(typeof(UsuarioActualDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Me(CancellationToken cancellationToken)
    {
        return Ok(await _usuarioActualHandler.HandleAsync(cancellationToken));
    }
}
