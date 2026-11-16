using AgroCita.Application.Features.Calidad;
using AgroCita.Application.Features.Fertilizacion;
using AgroCita.Application.Features.Fincas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroCita.API.Controllers.Fincas;

[ApiController]
[Route("api/cultivos")]
public class CultivosController(
    CultivosHandler cultivos,
    CalidadHandler calidad,
    FertilizacionHandler fertilizacion) : ControllerBase
{
    [HttpPut("{id:int}")]
    [Authorize(Roles = Permisos.Productor)]
    public async Task<ActionResult<CultivoDto>> Actualizar(
        int id, [FromBody] CultivoRequestDto dto, CancellationToken ct) =>
        Ok(await cultivos.ActualizarAsync(id, dto, ct));

    [HttpGet("{id:int}/inspecciones")]
    [Authorize(Roles = Permisos.Empresa)]
    public async Task<ActionResult<IReadOnlyList<InspeccionDto>>> Inspecciones(
        int id, CancellationToken ct) =>
        Ok(await calidad.ListarPorCultivoAsync(id, ct));

    [HttpGet("{id:int}/recomendaciones")]
    [Authorize(Roles = Permisos.Empresa)]
    public async Task<ActionResult<IReadOnlyList<RecomendacionDto>>> Recomendaciones(
        int id, CancellationToken ct) =>
        Ok(await fertilizacion.ListarRecomendacionesAsync(id, ct));

    [HttpPost("{id:int}/recomendaciones")]
    [Authorize(Roles = Permisos.Tecnico)]
    public async Task<ActionResult<RecomendacionDto>> CrearRecomendacion(
        int id, [FromBody] CrearRecomendacionDto dto, CancellationToken ct)
    {
        var recomendacion = await fertilizacion.CrearRecomendacionAsync(id, dto, ct);

        return Created($"/api/cultivos/{id}/recomendaciones", recomendacion);
    }

    [HttpGet("{id:int}/aplicaciones")]
    [Authorize(Roles = Permisos.Empresa)]
    public async Task<ActionResult<IReadOnlyList<AplicacionDto>>> Aplicaciones(
        int id, CancellationToken ct) =>
        Ok(await fertilizacion.ListarAplicacionesAsync(id, ct));

    [HttpPost("{id:int}/aplicaciones")]
    [Authorize(Roles = Permisos.Tecnico)]
    public async Task<ActionResult<AplicacionDto>> CrearAplicacion(
        int id, [FromBody] CrearAplicacionDto dto, CancellationToken ct)
    {
        var aplicacion = await fertilizacion.CrearAplicacionAsync(id, dto, ct);

        return Created($"/api/cultivos/{id}/aplicaciones", aplicacion);
    }
}
