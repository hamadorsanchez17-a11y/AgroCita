using AgroCita.Application.Features.Calidad;
using AgroCita.Application.Features.Fincas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroCita.API.Controllers.Fincas;

[ApiController]
[Route("api/tipos-cultivo")]
public class TiposCultivoController(CultivosHandler cultivos, CalidadHandler calidad) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = Permisos.Empresa)]
    public async Task<ActionResult<IReadOnlyList<TipoCultivoDto>>> Listar(CancellationToken ct) =>
        Ok(await cultivos.ListarTiposAsync(ct));

    [HttpPost]
    [Authorize(Roles = Permisos.Admin)]
    public async Task<ActionResult<TipoCultivoDto>> Crear(
        [FromBody] TipoCultivoRequestDto dto, CancellationToken ct)
    {
        var tipo = await cultivos.CrearTipoAsync(dto, ct);

        return Created("/api/tipos-cultivo", tipo);
    }

    [HttpGet("{id:int}/parametros")]
    [Authorize(Roles = Permisos.AdminOTecnico)]
    public async Task<ActionResult<IReadOnlyList<ParametroEvaluacionDto>>> Parametros(
        int id, CancellationToken ct) =>
        Ok(await calidad.ListarParametrosAsync(id, ct));

    [HttpPost("{id:int}/parametros")]
    [Authorize(Roles = Permisos.Admin)]
    public async Task<ActionResult<ParametroEvaluacionDto>> CrearParametro(
        int id, [FromBody] ParametroRequestDto dto, CancellationToken ct)
    {
        var parametro = await calidad.CrearParametroAsync(id, dto, ct);

        return Created($"/api/tipos-cultivo/{id}/parametros", parametro);
    }
}
