using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroCita.API.Controllers.Diagnostics;

[ApiController]
[Route("api/diagnostics")]
[Authorize]
public class AuthDiagnosticsController : ControllerBase
{
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        return Ok(new
        {
            mensaje = "Autenticación JWT exitosa.",
            userId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            email = User.FindFirstValue(ClaimTypes.Email),
            rol = User.FindFirstValue(ClaimTypes.Role),
            empresaId = User.FindFirstValue("company_id")
        });
    }
}
