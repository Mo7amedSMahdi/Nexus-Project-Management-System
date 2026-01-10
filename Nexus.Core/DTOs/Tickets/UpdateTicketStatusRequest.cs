namespace Nexus.Core.DTOs.Tickets;

public class UpdateTicketStatusRequest
{
    public int TicketId { get; set; }
    public string Status { get; set; } = string.Empty;
}