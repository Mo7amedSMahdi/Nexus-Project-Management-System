using Microsoft.EntityFrameworkCore;
using Nexus.Core.Interfaces.Tickets;
using Nexus.Core.Entities.Tickets;
using Nexus.Infrastructure.Data;

namespace Nexus.Infrastructure.Repositories.Tickets;

public class TicketRepository(NexusDbContext context) : ITicketRepository
{
    public async Task<List<Ticket>> GetByProjectIdAsync(int projectId)
    {
        return await context.Tickets.Where(t => t.ProjectId == projectId).ToListAsync();
    }

    public async Task<Ticket?> GetByIdAsync(int id)
    {
        return await context.Tickets.FindAsync(id);
    }

    public async Task AddAsync(Ticket ticket)
    {
        await context.Tickets.AddAsync(ticket);
        await context.SaveChangesAsync();
    }
}