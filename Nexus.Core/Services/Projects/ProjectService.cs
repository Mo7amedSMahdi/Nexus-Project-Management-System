using Nexus.Core.DTOs.Projects;
using Nexus.Core.Entities.Projects;
using Nexus.Core.Interfaces.Projects;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Nexus.Core.Services.Projects;

public class ProjectService(IProjectRepository repository, IHttpContextAccessor httpContextAccessor): IProjectService
{
    // Create a new project
    public async Task<ProjectResponse> CreateAsync(CreateProjectRequest request)
    {
        // Get the current user ID from the HttpContext
        var ownerId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if(string.IsNullOrEmpty(ownerId)) throw new UnauthorizedAccessException("User not authenticated");
        
        // Convert DTO to Domain Entity
        var project = new Project(request.Name, request.Code, ownerId, request.Description);
        
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
        // Get the project from the repository using the provided Id
        var ownerId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if(string.IsNullOrEmpty(ownerId)) throw new UnauthorizedAccessException("User not authenticated");
        
        var project = await repository.GetByIdAsync(id, ownerId);
        
        // Return null if the project is not found
        if (project == null) return null;
        
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
        // Get the current user ID from the HttpContext
        var ownerId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if(string.IsNullOrEmpty(ownerId)) throw new UnauthorizedAccessException("User not authenticated");
        
        var projects = await repository.GetByUserIdAsync(ownerId);
        
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