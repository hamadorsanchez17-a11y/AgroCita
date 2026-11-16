using AgroCita.Application.Features.Calidad;
using AgroCita.Application.Features.Fertilizacion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroCita.API.Controllers.Calidad;

[ApiController]
[Route("api")]
public class CalidadController(CalidadHandler calidad, FertilizacionHandler fertilizacion) : ControllerBase
{
    [HttpGet("categorias-calidad")]
    [Authorize(Roles = Permisos.AdminOTecnico)]
    public async Task<ActionResult<IReadOnlyList<CategoriaCalidadDto>>> Categorias(CancellationToken ct) =>
        Ok(await calidad.ListarCategoriasAsync(ct));

    [HttpPost("categorias-calidad")]
    [Authorize(Roles = Permisos.Admin)]
    public async Task<ActionResult<CategoriaCalidadDto>> CrearCategoria(
        [FromBody] CategoriaRequestDto dto, CancellationToken ct)
    {
        var categoria = await calidad.CrearCategoriaAsync(dto, ct);

        return Created("/api/categorias-calidad", categoria);
    }

    [HttpGet("inspecciones/{id:int}")]
    [Authorize(Roles = Permisos.Empresa)]
    public async Task<ActionResult<InspeccionDto>> Inspeccion(int id, CancellationToken ct) =>
        Ok(await calidad.ObtenerInspeccionAsync(id, ct));

    [HttpGet("fertilizantes")]
    [Authorize(Roles = Permisos.AdminOTecnico)]
    public async Task<ActionResult<IReadOnlyList<FertilizanteDto>>> Fertilizantes(CancellationToken ct) =>
        Ok(await fertilizacion.ListarFertilizantesAsync(ct));

    [HttpPost("fertilizantes")]
    [Authorize(Roles = Permisos.Admin)]
    public async Task<ActionResult<FertilizanteDto>> CrearFertilizante(
        [FromBody] FertilizanteRequestDto dto, CancellationToken ct)
    {
        var fertilizante = await fertilizacion.CrearFertilizanteAsync(dto, ct);

        return Created("/api/fertilizantes", fertilizante);
    }
}
