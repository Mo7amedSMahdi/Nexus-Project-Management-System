using Nexus.Core.Enums.Tickets;

namespace Nexus.Core.DTOs.Tickets;

public class CreateTicketRequest
{
    public int ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TicketPriority Priority { get; set; } = TicketPriority.Medium;
    
    public TicketStatus Status { get; set; } = TicketStatus.Todo;
}