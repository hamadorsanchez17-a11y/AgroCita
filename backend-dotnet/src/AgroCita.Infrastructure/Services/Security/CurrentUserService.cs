using System.Security.Claims;
using AgroCita.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AgroCita.Infrastructure.Services.Security;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public int? UsuarioId => Entero(JwtClaims.UsuarioId);

    public string? Rol => User?.FindFirst(JwtClaims.Rol)?.Value;

    public int? EmpresaId => Entero(JwtClaims.EmpresaId);

    private int? Entero(string claim) =>
        int.TryParse(User?.FindFirst(claim)?.Value, out var valor) ? valor : null;
}
