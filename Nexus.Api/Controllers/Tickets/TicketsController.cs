using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexus.Core.DTOs.Tickets;
using Nexus.Core.Interfaces.Tickets;

namespace Nexus.Api.Controllers.Tickets;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TicketsController(ITicketService ticketService) : ControllerBase
{
    // GET: api/tickets All Tickets by ProjectId
    ///<summary>
    /// List all available tickets for a project
    /// </summary>
    [HttpGet("project/{projectId:int}")]
    public async Task<ActionResult<List<TicketResponse>>> GetTicketsByProjectIdAsync(int projectId)
    {
        var tickets = await ticketService.GetTicketsByProjectIdAsync(projectId);
        return Ok(tickets);
    }
    
    // GET: api/tickets/ticketId
    ///<summary>
    /// Get a ticket by Id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TicketResponse?>> GetById(int id)
    {
        var ticket = await ticketService.GetByIdAsync(id);
        return ticket == null ? NotFound() : Ok(ticket);
    }
    
    // POST: api/tickets Create a new ticket
    ///<summary>
    /// Create a new ticket
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TicketResponse>> CreateAsync(CreateTicketRequest request)
    {
        var ticket = await ticketService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = ticket.Id }, ticket);
    }
    
    
    //PUT: api/tickets/updateStatus
    ///<summary>
    /// Update ticket status
    /// </summary>
    [HttpPut("updateStatus/{ticketId:int}/{status}")]
    public async Task<ActionResult<bool>> UpdateStatus(int ticketId, string status)
    {
        // We catch the UnauthorizedAccessException from the service automatically 
        // if you have global error handling, otherwise it returns 500. 
        // Ideally, wrapping this in try-catch for 403 is good practice if no global filter exists.
    
        var success = await ticketService.UpdateStatusAsync(ticketId, status);
    
        if (!success)
        {
            // Could mean Ticket Not Found OR Invalid Enum
            return BadRequest("Invalid Ticket ID or Status.");
        }
    
        return Ok(true);
    }
    
}