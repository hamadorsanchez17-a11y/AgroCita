using AgroCita.API.Filters;
using AgroCita.Infrastructure.Services.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroCita.API.Controllers.Diagnostics;

[ApiController]
[Route("api/diagnostics")]
[Authorize]
[SinValidarSuscripcion]
public class AuthDiagnosticsController : ControllerBase
{
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        return Ok(new
        {
            mensaje = "Autenticación JWT exitosa.",
            userId = User.FindFirst(JwtClaims.UsuarioId)?.Value,
            email = User.FindFirst(JwtClaims.Email)?.Value,
            rol = User.FindFirst(JwtClaims.Rol)?.Value,
            empresaId = User.FindFirst(JwtClaims.EmpresaId)?.Value
        });
    }
}
