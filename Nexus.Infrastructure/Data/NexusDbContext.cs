using Microsoft.EntityFrameworkCore;
using Nexus.Core.Entities.Projects;

namespace Nexus.Infrastructure.Data;

public class NexusDbContext(DbContextOptions<NexusDbContext> options) : DbContext(options)
{
    
    // Create a table named "Projects" with EF core
    public DbSet<Project> Projects { get; set; }
}