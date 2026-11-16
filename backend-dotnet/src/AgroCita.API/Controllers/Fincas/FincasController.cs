using AgroCita.Application.Features.Fincas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroCita.API.Controllers.Fincas;

[ApiController]
[Route("api/fincas")]
public class FincasController(FincasHandler fincas, CultivosHandler cultivos) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = Permisos.Productor)]
    public async Task<ActionResult<IReadOnlyList<FincaDto>>> ListarMias(CancellationToken ct) =>
        Ok(await fincas.ListarMiasAsync(ct));

    [HttpPost]
    [Authorize(Roles = Permisos.Productor)]
    public async Task<ActionResult<FincaDto>> Crear(
        [FromBody] FincaRequestDto dto, CancellationToken ct)
    {
        var finca = await fincas.CrearAsync(dto, ct);

        return Created($"/api/fincas/{finca.Id}", finca);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = Permisos.Empresa)]
    public async Task<ActionResult<FincaDetalleDto>> Obtener(int id, CancellationToken ct) =>
        Ok(await fincas.ObtenerAsync(id, ct));

    [HttpPut("{id:int}")]
    [Authorize(Roles = Permisos.Productor)]
    public async Task<ActionResult<FincaDto>> Actualizar(
        int id, [FromBody] FincaRequestDto dto, CancellationToken ct) =>
        Ok(await fincas.ActualizarAsync(id, dto, ct));

    [HttpGet("{fincaId:int}/cultivos")]
    [Authorize(Roles = Permisos.Empresa)]
    public async Task<ActionResult<IReadOnlyList<CultivoDto>>> ListarCultivos(
        int fincaId, CancellationToken ct) =>
        Ok(await cultivos.ListarPorFincaAsync(fincaId, ct));

    [HttpPost("{fincaId:int}/cultivos")]
    [Authorize(Roles = Permisos.Productor)]
    public async Task<ActionResult<CultivoDto>> CrearCultivo(
        int fincaId, [FromBody] CultivoRequestDto dto, CancellationToken ct)
    {
        var cultivo = await cultivos.CrearAsync(fincaId, dto, ct);

        return Created($"/api/fincas/{fincaId}/cultivos", cultivo);
    }
}
