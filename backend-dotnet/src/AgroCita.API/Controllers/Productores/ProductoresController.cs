using AgroCita.Application.Common.Models;
using AgroCita.Application.Features.Informes;
using AgroCita.Application.Features.Productores;
using AgroCita.Application.Features.Visitas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroCita.API.Controllers.Productores;

[ApiController]
[Route("api/productores")]
public class ProductoresController(
    ProductoresHandler productores,
    InformesHandler informes,
    VisitasHandler visitas) : ControllerBase
{
    [HttpGet("me")]
    [Authorize(Roles = Permisos.Productor)]
    public async Task<ActionResult<ProductorDto>> ObtenerMiPerfil(CancellationToken ct) =>
        Ok(await productores.ObtenerMiPerfilAsync(ct));

    [HttpPut("me")]
    [Authorize(Roles = Permisos.Productor)]
    public async Task<ActionResult<ProductorDto>> ActualizarMiPerfil(
        [FromBody] ActualizarProductorDto dto, CancellationToken ct) =>
        Ok(await productores.ActualizarMiPerfilAsync(dto, ct));

    [HttpGet("me/informes")]
    [Authorize(Roles = Permisos.Productor)]
    public async Task<ActionResult<IReadOnlyList<InformeResumenDto>>> MisInformes(
        [FromQuery] int? fincaId, CancellationToken ct) =>
        Ok(await informes.ListarMiosAsync(fincaId, ct));

    [HttpGet("me/historial")]
    [Authorize(Roles = Permisos.Productor)]
    public async Task<ActionResult<HistorialDto>> MiHistorial(
        [FromQuery] int? fincaId, [FromQuery] int? cultivoId, CancellationToken ct) =>
        Ok(await informes.HistorialAsync(null, fincaId, cultivoId, ct));

    [HttpGet]
    [Authorize(Roles = Permisos.Admin)]
    public async Task<ActionResult<PaginadoDto<ProductorResumenDto>>> Listar(
        [FromQuery] string? buscar,
        [FromQuery] int? pagina,
        [FromQuery] int? tamano,
        CancellationToken ct) =>
        Ok(await productores.ListarAsync(buscar, pagina, tamano, ct));

    [HttpGet("{id:int}")]
    [Authorize(Roles = Permisos.Admin)]
    public async Task<ActionResult<ProductorDetalleDto>> Obtener(int id, CancellationToken ct) =>
        Ok(await productores.ObtenerAsync(id, ct));

    [HttpGet("{id:int}/visitas")]
    [Authorize(Roles = Permisos.Admin)]
    public async Task<ActionResult<IReadOnlyList<VisitaDto>>> Visitas(int id, CancellationToken ct) =>
        Ok(await visitas.ListarPorProductorAsync(id, ct));
}
