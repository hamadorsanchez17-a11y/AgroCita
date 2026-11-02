
using Microsoft.EntityFrameworkCore;

namespace AgroCita.Infrastructure.Persistence.Context;

public class AgroCitaDbContext : DbContext
{
    public AgroCitaDbContext(
        DbContextOptions<AgroCitaDbContext> options)
        : base(options)
    {
    }
}
