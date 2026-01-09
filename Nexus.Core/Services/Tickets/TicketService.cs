using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Nexus.Core.DTOs.Tickets;
using Nexus.Core.Interfaces.Tickets;
using Nexus.Core.Entities.Tickets;
using Nexus.Core.Interfaces.Projects;
using Nexus.Core.Interfaces.Security;

namespace Nexus.Core.Services.Tickets;

public class TicketService(ITicketRepository ticketRepository,ICurrentUser currentUser,IPermissionService permissionService,IProjectRepository projectRepository) : ITicketService
{
    public async Task<List<TicketResponse>> GetTicketsByProjectIdAsync(int projectId)
    {
        if(!permissionService.IsAuthenticated()) throw new UnauthorizedAccessException("User not authenticated");
        
        var project = await projectRepository.GetByIdAsync(projectId);
        if(project == null) throw new Exception("Project not found");
        
        if(!permissionService.CanAccessProject(currentUser.UserId,project)) throw new UnauthorizedAccessException("User not authorized to access this project");
        
        var tickets = await ticketRepository.GetByProjectIdAsync(projectId);
        return tickets.Select(MapToResponse).ToList();
    }

    public async Task<TicketResponse> CreateAsync(CreateTicketRequest request)
    {
        if(!permissionService.IsAuthenticated()) throw new UnauthorizedAccessException("User not authenticated");
        
        var project = await projectRepository.GetByIdAsync(request.ProjectId);
        if(project == null) throw new Exception("Project not found");
        
        if(!permissionService.CanAccessProject(currentUser.UserId,project)) throw new UnauthorizedAccessException("User not authorized to access this project");
        
        var ticket = new Ticket(title:request.Title, projectId:request.ProjectId, priority:request.Priority)
        {
            Description = request.Description
        };
        await ticketRepository.AddAsync(ticket);
        return MapToResponse(ticket);
    }

    public async Task<TicketResponse?> GetByIdAsync(int id)
    {
        if(!permissionService.IsAuthenticated()) throw new UnauthorizedAccessException("User not authenticated");
        
        var ticket = await ticketRepository.GetByIdAsync(id);
        if (ticket == null) return null;
        
        var project = await projectRepository.GetByIdAsync(ticket.ProjectId);
        if(project == null) throw new Exception("Project not found");
        
        if(!permissionService.CanAccessProject(currentUser.UserId,project)) throw new UnauthorizedAccessException("User not authorized to access this project");
        
        return MapToResponse(ticket);
    }

    private static TicketResponse MapToResponse(Ticket ticket)
    {
        return new TicketResponse
            {
            Id = ticket.Id,
            ProjectId = ticket.ProjectId,
            Title = ticket.Title,
            Description = ticket.Description,
            Status = ticket.Status.ToString(),
            Priority = ticket.Priority.ToString(),
            CreatedAt = ticket.CreatedAt
        };
    }
}