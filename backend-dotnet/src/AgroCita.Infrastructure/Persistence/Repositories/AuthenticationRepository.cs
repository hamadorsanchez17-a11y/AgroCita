using AgroCita.Application.Features.Auth.Login;
using AgroCita.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AgroCita.Infrastructure.Persistence.Repositories;

public class AuthenticationRepository : IAuthenticationRepository
{
    private readonly AgroCitaDbContext _context;

    public AuthenticationRepository(AgroCitaDbContext context)
    {
        _context = context;
    }

    public async Task<AuthenticationUser?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Where(user => user.Email == email)
            .Select(user => new AuthenticationUser
            {
                Id = user.Id,
                Nombre = (user.Nombre + " " + user.Apellido).Trim(),
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                Role = user.Rol!.Nombre,
                CompanyId = user.EmpresaId,
                CompanyName = user.Empresa != null ? user.Empresa.Nombre : null,
                IsActive = user.Activo
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
