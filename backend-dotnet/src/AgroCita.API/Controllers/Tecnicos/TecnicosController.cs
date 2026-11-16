using AgroCita.Application.Features.Tecnicos;
using AgroCita.Application.Features.Visitas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroCita.API.Controllers.Tecnicos;

[ApiController]
[Route("api/tecnicos")]
[Authorize(Roles = Permisos.Admin)]
public class TecnicosController(TecnicosHandler tecnicos, VisitasHandler visitas) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TecnicoDto>>> Listar(CancellationToken ct) =>
        Ok(await tecnicos.ListarAsync(ct));

    [HttpPost]
    public async Task<ActionResult<TecnicoDto>> Crear(
        [FromBody] CrearTecnicoDto dto, CancellationToken ct)
    {
        var tecnico = await tecnicos.CrearAsync(dto, ct);

        return Created("/api/tecnicos", tecnico);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TecnicoDto>> Actualizar(
        int id, [FromBody] ActualizarTecnicoDto dto, CancellationToken ct) =>
        Ok(await tecnicos.ActualizarAsync(id, dto, ct));

    [HttpPatch("{id:int}/estado")]
    public async Task<IActionResult> CambiarEstado(
        int id, [FromBody] CambiarEstadoUsuarioDto dto, CancellationToken ct)
    {
        await tecnicos.CambiarEstadoAsync(id, dto, ct);

        return NoContent();
    }

    [HttpGet("{id:int}/agenda")]
    public async Task<ActionResult<IReadOnlyList<VisitaAgendaDto>>> Agenda(
        int id, [FromQuery] DateOnly? desde, [FromQuery] DateOnly? hasta, CancellationToken ct) =>
        Ok(await visitas.AgendaDeTecnicoAsync(id, desde, hasta, ct));
}
