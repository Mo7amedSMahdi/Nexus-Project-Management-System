using Microsoft.EntityFrameworkCore;
using Nexus.Core.Entities.Projects;
using Nexus.Core.Interfaces.Projects;
using Nexus.Infrastructure.Data;
   

namespace Nexus.Infrastructure.Repositories.Projects;

public class ProjectRepository(NexusDbContext context) : IProjectRepository
{
   // Find a project by id
   public async Task<Project?> GetByIdAsync(int id)
   {
      return await context.Projects.FindAsync(id);
   }

   // Return the list of projects
   public async Task<List<Project>> GetAllAsync()
   {
      return await context.Projects.ToListAsync();
   }
   
   // Add a new project
   public async Task AddAsync(Project project)
   {
      await context.Projects.AddAsync(project);
      await context.SaveChangesAsync();
   }

   public async Task<List<Project>> GetByUserIdAsync(string id)
   {
      return await context.Projects.Where(p => p.OwnerId == id).ToListAsync();
   }
}