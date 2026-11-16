using AgroCita.Application.Features.Informes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroCita.API.Controllers.Informes;

[ApiController]
[Route("api/historial")]
[Authorize(Roles = Permisos.Admin)]
public class HistorialController(InformesHandler informes) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<HistorialDto>> Obtener(
        [FromQuery] int? productorId,
        [FromQuery] int? fincaId,
        [FromQuery] int? cultivoId,
        CancellationToken ct) =>
        Ok(await informes.HistorialAsync(productorId, fincaId, cultivoId, ct));
}
