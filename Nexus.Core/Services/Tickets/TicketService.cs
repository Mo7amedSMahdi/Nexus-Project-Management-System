using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Nexus.Core.DTOs.Tickets;
using Nexus.Core.Interfaces.Tickets;
using Nexus.Core.Entities.Tickets;
using Nexus.Core.Interfaces.Projects;

namespace Nexus.Core.Services.Tickets;

public class TicketService(ITicketRepository ticketRepository,HttpContextAccessor httpContextAccessor,IProjectRepository projectRepository) : ITicketService
{
    public async Task<List<TicketResponse>> GetTicketsByProjectIdAsync(int projectId)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(userId == null) throw new UnauthorizedAccessException("User not authenticated");
        var project = await projectRepository.GetByIdAsync(projectId, userId);
        if(project == null) throw new Exception("Project not found");
        
        var tickets = await ticketRepository.GetByProjectIdAsync(projectId);
        return tickets.Select(MapToResponse).ToList();
    }

    public async Task<TicketResponse> CreateAsync(CreateTicketRequest request)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(userId == null) throw new UnauthorizedAccessException("User not authenticated");
        
        var project = await projectRepository.GetByIdAsync(request.ProjectId,userId);
        if(project == null) throw new Exception("Project not found");
        
        var ticket = new Ticket(title:request.Title, projectId:request.ProjectId, priority:request.Priority)
        {
            Description = request.Description
        };
        await ticketRepository.AddAsync(ticket);
        return MapToResponse(ticket);
    }

    public async Task<TicketResponse?> GetByIdAsync(int id)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(userId == null) throw new UnauthorizedAccessException("User not authenticated");
        
        var ticket = await ticketRepository.GetByIdAsync(id);
        if (ticket == null) return null;
        var project = await projectRepository.GetByIdAsync(ticket.ProjectId,userId);
        
        return project == null ? null : MapToResponse(ticket);
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