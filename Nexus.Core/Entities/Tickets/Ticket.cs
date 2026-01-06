using Nexus.Core.Entities.Projects;
using Nexus.Core.Enums.Tickets;

namespace Nexus.Core.Entities.Tickets;

public class Ticket(string title, int projectId,string description = null, TicketStatus status = TicketStatus.Todo, TicketPriority priority = TicketPriority.Medium) : BaseEntity
{
    public string Title { get; set; } = title;
    public string? Description { get; set; } = description;
    public TicketStatus Status { get; set; } = status;
    public TicketPriority Priority { get; set; } = priority;
    
    // Add relationship with Project
    
    public int ProjectId { get; set; } = projectId;
    // Virtual allows EF core to be smart and lazy load the project
    public virtual Project Project { get; set;}
    
}