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

    public int? EmpresaId
    {
        get
        {
            var empresaId = _httpContextAccessor.HttpContext?
                .User.FindFirst("company_id")?.Value;

            return int.TryParse(empresaId, out var id) ? id : null;
        }
    }
}