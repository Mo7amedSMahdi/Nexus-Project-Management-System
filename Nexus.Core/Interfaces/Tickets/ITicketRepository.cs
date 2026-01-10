using Nexus.Core.Entities.Tickets;

namespace Nexus.Core.Interfaces.Tickets;

public interface ITicketRepository
{
    Task<List<Ticket>> GetByProjectIdAsync(int projectId);
    Task <Ticket?> GetByIdAsync(int id);
    Task AddAsync(Ticket ticket);
    
    Task UpdateAsync(Ticket ticket);
}