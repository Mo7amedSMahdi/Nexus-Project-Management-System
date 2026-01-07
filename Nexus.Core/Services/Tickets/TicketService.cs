using Nexus.Core.DTOs.Tickets;
using Nexus.Core.Interfaces.Tickets;
using Nexus.Core.Entities.Tickets;

namespace Nexus.Core.Services.Tickets;

public class TicketService(ITicketRepository ticketRepository) : ITicketService
{
    public async Task<List<TicketResponse>> GetTicketsByProjectIdAsync(int projectId)
    {
        var tickets = await ticketRepository.GetByProjectIdAsync(projectId);
        return tickets.Select(MapToResponse).ToList();
    }

    public async Task<TicketResponse> CreateAsync(CreateTicketRequest request)
    {
        var ticket = new Ticket(title:request.Title, projectId:request.ProjectId, priority:request.Priority)
        {
            Description = request.Description
        };
        await ticketRepository.AddAsync(ticket);
        return MapToResponse(ticket);
    }

    public async Task<TicketResponse?> GetByIdAsync(int id)
    {
        var ticket = await ticketRepository.GetByIdAsync(id);
        return ticket == null ? null : MapToResponse(ticket);
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