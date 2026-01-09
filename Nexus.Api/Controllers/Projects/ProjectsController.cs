using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexus.Core.Interfaces.Projects;
using Nexus.Core.DTOs.Projects;

namespace Nexus.Api.Controllers.Projects;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ProjectsController(IProjectService projectService) : ControllerBase
{
    /// <summary>
    /// Get all projects
    /// </summary>
    // GET: api/projects
    [HttpGet]
    
    public async Task<ActionResult<List<ProjectResponse>>> GetAll()
    {
        var projects = await projectService.GetAllAsync();
        return Ok(projects);
    }

    ///<summary>
    /// Get project by ownerId
    /// </summary>
    // GET: api/projects/{ownerId}
    [HttpGet($"MyProjects")]
    public async Task<ActionResult<List<ProjectResponse>>> GetByUserId()
    {
        var projects = await projectService.GetByUserIdAsync();
        return Ok(projects);
        
    }

    ///<summary>
    /// Get project by Id
    /// </summary>
    // GET: api/projects/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProjectResponse?>> GetById(int id)
    {
        var project = await projectService.GetByIdAsync(id);
        if(project == null) return NotFound();
        
        return Ok(project);
    }

    /// <summary>
    /// Create a new project
    /// </summary>
    // POST: api/projects
    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectRequest request)
    {
        var project = await projectService.CreateAsync(request);

        return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
    }
}