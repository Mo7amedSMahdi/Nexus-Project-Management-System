using Nexus.Core.DTOs.Tickets;

namespace Nexus.Core.Interfaces.Tickets;

public interface ITicketService
{
    Task<List<TicketResponse>> GetTicketsByProjectIdAsync(int projectId);
    Task<TicketResponse> CreateAsync(CreateTicketRequest request);
    Task<TicketResponse?> GetByIdAsync(int id);
}