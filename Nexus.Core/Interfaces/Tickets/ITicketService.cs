using Nexus.Core.DTOs.Tickets;
using Nexus.Core.Entities.Tickets;

namespace Nexus.Core.Interfaces.Tickets;

public interface ITicketService
{
    Task<List<TicketResponse>> GetTicketsByProjectIdAsync(int projectId);
    Task<TicketResponse> CreateAsync(CreateTicketRequest request);
    Task<TicketResponse?> GetByIdAsync(int id);

    Task<bool> UpdateStatusAsync(int ticketID, string status);
}