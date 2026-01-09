using Nexus.Core.DTOs.Projects;
using Nexus.Core.Entities.Projects;
using Nexus.Core.Interfaces.Projects;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Nexus.Core.Interfaces.Security;

namespace Nexus.Core.Services.Projects;

public class ProjectService(IProjectRepository repository,ICurrentUser currentUser,IPermissionService permissionService): IProjectService
{
    // Create a new project
    public async Task<ProjectResponse> CreateAsync(CreateProjectRequest request)
    {
        // Get the current user ID from the HttpContext
        if(!permissionService.IsAuthenticated()) throw new UnauthorizedAccessException("User not authenticated");
        
        // Convert DTO to Domain Entity
        var project = new Project(request.Name, request.Code, currentUser.UserId, request.Description);
        
        // Save to Database via Repository
        await repository.AddAsync(project);
        
        // Convert back to DTO to return it to the user
        return new ProjectResponse
        {
            Id = project.Id,
            Name = project.Name,
            Code = project.Code,
            OwnerId = project.OwnerId,
            Description = project.Description,
            CreatedAt = project.CreatedAt
        };
    }
    
    // Get all the projects
    public async Task<List<ProjectResponse>> GetAllAsync()
    {
        if(!permissionService.IsAuthenticated()) throw new UnauthorizedAccessException("User not authenticated");
        
        if (!permissionService.isAdmin(currentUser.UserId))
            throw new UnauthorizedAccessException("User not authorized to access this resource");
        // Get the projects from the repository
        var projects = await repository.GetAllAsync();
        
        // Convert List<Entity> to List<DTO>
        return projects.Select(p => new ProjectResponse
        {
            Id = p.Id,
            OwnerId = p.OwnerId,
            Name = p.Name,
            Code = p.Code,
            Description = p.Description,
            CreatedAt = p.CreatedAt
        }).ToList();
    }
    
    // Get a project by id
    public async Task<ProjectResponse?> GetByIdAsync(int id)
    {
        if(!permissionService.IsAuthenticated()) throw new UnauthorizedAccessException("User not authenticated");
        
        var project = await repository.GetByIdAsync(id);
        // Return null if the project is not found
        if (project == null) return null;
        
        
        if(!permissionService.CanAccessProject(currentUser.UserId,project)) throw new UnauthorizedAccessException("User not authorized to access this project");
        
        // Convert Entity to DTO
        return new ProjectResponse
        {
            Id = project.Id,
            OwnerId = project.OwnerId,
            Name = project.Name,
            Code = project.Code,
            Description = project.Description,
            CreatedAt = project.CreatedAt
        };
    }

    public async Task<List<ProjectResponse>> GetByUserIdAsync()
    {
        if(!permissionService.IsAuthenticated()) throw new UnauthorizedAccessException("User not authenticated");
        
        var projects = await repository.GetByUserIdAsync(currentUser.UserId);
        
        return projects.Select(p => new ProjectResponse
        {
            Id = p.Id,
            OwnerId = p.OwnerId,
            Name = p.Name,
            Code = p.Code,
            Description = p.Description,
            CreatedAt = p.CreatedAt
        }).ToList();
    }
}