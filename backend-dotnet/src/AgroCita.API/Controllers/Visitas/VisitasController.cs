using AgroCita.Application.Features.Calidad;
using AgroCita.Application.Features.Informes;
using AgroCita.Application.Features.Visitas;
using AgroCita.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroCita.API.Controllers.Visitas;

[ApiController]
[Route("api/visitas")]
public class VisitasController(
    VisitasHandler visitas,
    CalidadHandler calidad,
    InformesHandler informes) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = Permisos.Admin)]
    public async Task<ActionResult<VisitaDto>> Crear(
        [FromBody] CrearVisitaDto dto, CancellationToken ct)
    {
        var visita = await visitas.CrearAsync(dto, ct);

        return Created($"/api/visitas/{visita.Id}", visita);
    }

    [HttpGet("agenda")]
    [Authorize(Roles = Permisos.Admin)]
    public async Task<ActionResult<IReadOnlyList<VisitaAgendaDto>>> Agenda(
        [FromQuery] DateOnly? desde,
        [FromQuery] DateOnly? hasta,
        [FromQuery] int? tecnicoId,
        CancellationToken ct) =>
        Ok(await visitas.AgendaAsync(desde, hasta, tecnicoId, ct));

    [HttpGet("mias")]
    [Authorize(Roles = Permisos.Tecnico)]
    public async Task<ActionResult<IReadOnlyList<VisitaDto>>> ListarMias(
        [FromQuery] VisitStatus? estado,
        [FromQuery] DateOnly? desde,
        [FromQuery] DateOnly? hasta,
        CancellationToken ct) =>
        Ok(await visitas.ListarMiasAsync(estado, desde, hasta, ct));

    [HttpGet("{id:int}")]
    [Authorize(Roles = Permisos.Empresa)]
    public async Task<ActionResult<VisitaDetalleDto>> Obtener(int id, CancellationToken ct) =>
        Ok(await visitas.ObtenerAsync(id, ct));

    [HttpPatch("{id:int}/estado")]
    [Authorize(Roles = Permisos.AdminOTecnico)]
    public async Task<ActionResult<VisitaDto>> CambiarEstado(
        int id, [FromBody] CambiarEstadoVisitaDto dto, CancellationToken ct) =>
        Ok(await visitas.CambiarEstadoAsync(id, dto, ct));

    [HttpPut("{id:int}/reprogramar")]
    [Authorize(Roles = Permisos.Admin)]
    public async Task<ActionResult<VisitaDto>> Reprogramar(
        int id, [FromBody] ReprogramarVisitaDto dto, CancellationToken ct) =>
        Ok(await visitas.ReprogramarAsync(id, dto, ct));

    [HttpGet("{id:int}/reprogramaciones")]
    [Authorize(Roles = Permisos.Admin)]
    public async Task<ActionResult<IReadOnlyList<ReprogramacionDto>>> Reprogramaciones(
        int id, CancellationToken ct) =>
        Ok(await visitas.ListarReprogramacionesAsync(id, ct));

    [HttpPost("{visitaId:int}/inspecciones")]
    [Authorize(Roles = Permisos.Tecnico)]
    public async Task<ActionResult<InspeccionDto>> CrearInspeccion(
        int visitaId, [FromBody] CrearInspeccionDto dto, CancellationToken ct)
    {
        var inspeccion = await calidad.CrearInspeccionAsync(visitaId, dto, ct);

        return Created($"/api/inspecciones/{inspeccion.Id}", inspeccion);
    }

    [HttpPost("{id:int}/informe")]
    [Authorize(Roles = Permisos.Tecnico)]
    public async Task<ActionResult<InformeDto>> CrearInforme(
        int id, [FromBody] CrearInformeDto dto, CancellationToken ct)
    {
        var informe = await informes.CrearAsync(id, dto, ct);

        return Created($"/api/visitas/{id}/informe", informe);
    }

    [HttpGet("{id:int}/informe")]
    [Authorize(Roles = Permisos.Empresa)]
    public async Task<ActionResult<InformeDto>> ObtenerInforme(int id, CancellationToken ct) =>
        Ok(await informes.ObtenerAsync(id, ct));
}
