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
    [HttpGet("project/{projectId}")]
    public async Task<ActionResult<List<TicketResponse>>> GetTicketsByProjectIdAsync(int projectId)
    {
        var tickets = await ticketService.GetTicketsByProjectIdAsync(projectId);
        return Ok(tickets);
    }
    
    // GET: api/tickets/ticketId
    ///<summary>
    /// Get a ticket by Id
    /// </summary>
    [HttpGet("{id}")]
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
    
}