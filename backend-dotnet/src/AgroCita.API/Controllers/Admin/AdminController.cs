using AgroCita.Application.Common.Models;
using AgroCita.Application.Features.Admin;
using AgroCita.Application.Features.Empresa;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroCita.API.Controllers.Admin;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = Permisos.SuperAdmin)]
public class AdminController(AdminHandler admin) : ControllerBase
{
    [HttpGet("planes")]
    public async Task<ActionResult<IReadOnlyList<PlanDto>>> Planes(CancellationToken ct) =>
        Ok(await admin.ListarPlanesAsync(ct));

    [HttpPost("planes")]
    public async Task<ActionResult<PlanDto>> CrearPlan(
        [FromBody] PlanRequestDto dto, CancellationToken ct)
    {
        var plan = await admin.CrearPlanAsync(dto, ct);

        return Created("/api/admin/planes", plan);
    }

    [HttpPut("planes/{id:int}")]
    public async Task<ActionResult<PlanDto>> ActualizarPlan(
        int id, [FromBody] PlanRequestDto dto, CancellationToken ct) =>
        Ok(await admin.ActualizarPlanAsync(id, dto, ct));

    [HttpGet("empresas")]
    public async Task<ActionResult<PaginadoDto<EmpresaResumenDto>>> Empresas(
        [FromQuery] int? pagina, [FromQuery] int? tamano, CancellationToken ct) =>
        Ok(await admin.ListarEmpresasAsync(pagina, tamano, ct));

    [HttpPost("empresas")]
    public async Task<ActionResult<EmpresaDto>> CrearEmpresa(
        [FromBody] CrearEmpresaDto dto, CancellationToken ct)
    {
        var empresa = await admin.CrearEmpresaAsync(dto, ct);

        return Created("/api/admin/empresas", empresa);
    }

    [HttpPut("empresas/{id:int}/suscripcion")]
    public async Task<ActionResult<SuscripcionDto>> AsignarPlan(
        int id, [FromBody] AsignarPlanDto dto, CancellationToken ct) =>
        Ok(await admin.AsignarPlanAsync(id, dto, ct));

    [HttpPatch("suscripciones/{id:int}/estado")]
    public async Task<ActionResult<SuscripcionDto>> CambiarEstadoSuscripcion(
        int id, [FromBody] CambiarEstadoSuscripcionDto dto, CancellationToken ct) =>
        Ok(await admin.CambiarEstadoSuscripcionAsync(id, dto, ct));
}
