using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nexus.Core.Entities.Identity;
using Nexus.Core.Entities.Projects;
using Nexus.Core.Entities.Tickets;

namespace Nexus.Infrastructure.Data;

public class NexusDbContext(DbContextOptions<NexusDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    
    // Create a table named "Projects" with EF core
    public DbSet<Project> Projects { get; set; }
    public DbSet<Ticket> Tickets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}