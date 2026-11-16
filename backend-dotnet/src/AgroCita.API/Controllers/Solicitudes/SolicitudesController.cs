using AgroCita.Application.Common.Models;
using AgroCita.Application.Features.Solicitudes;
using AgroCita.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroCita.API.Controllers.Solicitudes;

[ApiController]
[Route("api/solicitudes")]
public class SolicitudesController(SolicitudesHandler solicitudes) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = Permisos.Productor)]
    public async Task<ActionResult<SolicitudDto>> Crear(
        [FromBody] CrearSolicitudDto dto, CancellationToken ct)
    {
        var solicitud = await solicitudes.CrearAsync(dto, ct);

        return Created($"/api/solicitudes/{solicitud.Id}", solicitud);
    }

    [HttpGet("mias")]
    [Authorize(Roles = Permisos.Productor)]
    public async Task<ActionResult<IReadOnlyList<SolicitudDto>>> ListarMias(
        [FromQuery] AssistanceRequestStatus? estado, CancellationToken ct) =>
        Ok(await solicitudes.ListarMiasAsync(estado, ct));

    [HttpGet]
    [Authorize(Roles = Permisos.Admin)]
    public async Task<ActionResult<PaginadoDto<SolicitudDto>>> Listar(
        [FromQuery] AssistanceRequestStatus? estado,
        [FromQuery] DateOnly? desde,
        [FromQuery] DateOnly? hasta,
        [FromQuery] int? pagina,
        [FromQuery] int? tamano,
        CancellationToken ct) =>
        Ok(await solicitudes.ListarAsync(estado, desde, hasta, pagina, tamano, ct));

    [HttpGet("{id:int}")]
    [Authorize(Roles = Permisos.AdminOProductor)]
    public async Task<ActionResult<SolicitudDetalleDto>> Obtener(int id, CancellationToken ct) =>
        Ok(await solicitudes.ObtenerAsync(id, ct));

    [HttpPatch("{id:int}/estado")]
    [Authorize(Roles = Permisos.Admin)]
    public async Task<ActionResult<SolicitudDto>> CambiarEstado(
        int id, [FromBody] CambiarEstadoSolicitudDto dto, CancellationToken ct) =>
        Ok(await solicitudes.CambiarEstadoAsync(id, dto, ct));
}
