using AgroCita.Application.Common.Models;
using AgroCita.Domain.Entities;

namespace AgroCita.Application.Features.Empresa;

public record EmpresaDto(
    int Id,
    string Nombre,
    string Rtn,
    string Email,
    string Telefono,
    string? Direccion,
    string CodigoInvitacion,
    bool Activa);

public record SuscripcionDto(
    int Id,
    ReferenciaDto Empresa,
    ReferenciaDto Plan,
    DateOnly FechaInicio,
    DateOnly? FechaFin,
    SubscriptionStatus Estado);

public static class EmpresaMapeos
{
    public static EmpresaDto ADto(this Company empresa) => new(
        empresa.Id,
        empresa.Nombre,
        empresa.Rtn,
        empresa.Email,
        empresa.Telefono,
        empresa.Direccion,
        empresa.CodigoInvitacion,
        empresa.Activa);

    public static SuscripcionDto ADto(this Subscription suscripcion) => new(
        suscripcion.Id,
        new ReferenciaDto(suscripcion.EmpresaId, suscripcion.Empresa?.Nombre ?? string.Empty),
        new ReferenciaDto(suscripcion.PlanId, suscripcion.Plan?.Nombre ?? string.Empty),
        suscripcion.FechaInicio,
        suscripcion.FechaFin,
        suscripcion.Estado);
}
