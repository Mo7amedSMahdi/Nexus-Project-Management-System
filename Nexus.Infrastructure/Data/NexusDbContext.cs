using Microsoft.EntityFrameworkCore;

namespace Nexus.Infrastructure.Data;

public class NexusDbContext(DbContextOptions<NexusDbContext> options) : DbContext(options)
{
}