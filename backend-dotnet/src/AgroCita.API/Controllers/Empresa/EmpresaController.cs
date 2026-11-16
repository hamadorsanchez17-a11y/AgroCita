using AgroCita.API.Filters;
using AgroCita.Application.Features.Empresa;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroCita.API.Controllers.Empresa;

[ApiController]
[Route("api/empresa")]
[Authorize(Roles = Permisos.Admin)]
public class EmpresaController(EmpresaHandler empresa) : ControllerBase
{
    [HttpGet]
    [SinValidarSuscripcion]
    public async Task<ActionResult<EmpresaDto>> Obtener(CancellationToken ct) =>
        Ok(await empresa.ObtenerAsync(ct));

    [HttpPost("codigo-invitacion")]
    public async Task<ActionResult<EmpresaDto>> RegenerarCodigo(CancellationToken ct) =>
        Ok(await empresa.RegenerarCodigoAsync(ct));

    [HttpGet("suscripcion")]
    [SinValidarSuscripcion]
    public async Task<ActionResult<SuscripcionDto>> Suscripcion(CancellationToken ct) =>
        Ok(await empresa.ObtenerSuscripcionAsync(ct));
}
