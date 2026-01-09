using Nexus.Core.Entities;
using Nexus.Core.Entities.Projects;

namespace Nexus.Core.Interfaces.Projects;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(int id,string ownerId);
    Task<List<Project>> GetAllAsync();
    Task AddAsync(Project project);
    
    Task<List<Project>> GetByUserIdAsync(string id);
}