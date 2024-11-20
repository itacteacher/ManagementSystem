using ManagementSystem.Application.Tickets.Commands.Create;
using ManagementSystem.Application.Tickets.Commands.Update;
using ManagementSystem.Application.Tickets.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ManagementSystem.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TicketsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TicketsController (IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get details of a specific ticket by Id.
    /// </summary>
    /// <param name="ticketId">The Id of the ticket.</param>
    /// <returns>The details of the specified ticket.</returns>
    /// <response code="200">Returns the ticket details.</response>
    /// <response code="404">If the ticket is not found.</response>
    [HttpGet("{ticketId:guid}")]
    public async Task<IActionResult> GetTicketDetails (Guid ticketId)
    {
        var query = new GetTicketDetailsQuery { Id = ticketId };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound("Ticket was not found.");
        }

        return Ok(result);
    }

    /// <summary>
    /// Create a new ticket.
    /// </summary>
    /// <param name="command">The command to create a ticket.</param>
    /// <returns>The Id of the created ticket.</returns>
    /// <response code="200">Returns the Id of the newly created ticket.</response>
    /// <response code="400">If the request is invalid or an error occurs.</response>
    [HttpPost]
    public async Task<IActionResult> CreateTicket ([FromBody] CreateTicketCommand command)
    {
        try
        {
            var ticketId = await _mediator.Send(command);

            return Ok(ticketId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Update an existing ticket.
    /// </summary>
    /// <param name="id">The Id of the ticket to update.</param>
    /// <param name="command">The command containing updated ticket data.</param>
    /// <response code="204">Ticket successfully updated.</response>
    /// <response code="400">If the request is invalid or Ids do not match.</response>
    /// <response code="404">If the ticket is not found.</response>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateTicket (Guid id, [FromBody] UpdateTicketCommand command)
    {
        if (id != command.TicketId)
        {
            return BadRequest("Ticket Id in URL does not match Id in request body.");
        }

        try
        {
            await _mediator.Send(command);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
