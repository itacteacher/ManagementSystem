using ManagementSystem.Application.Common.Models;
using ManagementSystem.Application.Tickets.Queries.GetByUserId;
using ManagementSystem.Application.Users;
using ManagementSystem.Application.Users.Commands.Create;
using ManagementSystem.Application.Users.Commands.Delete;
using ManagementSystem.Application.Users.Commands.Update;
using ManagementSystem.Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ManagementSystem.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController (IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all users.
    /// </summary>
    /// <returns>A list of all users.</returns>
    /// <response code="200">Returns the list of users.</response>
    /// <response code="404">If no users are found.</response>
    [HttpGet]
    public async Task<ActionResult<List<UserDTO>>> GetAllUsers ()
    {
        var query = new GetAllUsersQuery();
        var users = await _mediator.Send(query);

        if (users is null or [])
        {
            return NotFound("User was not found.");
        }

        return Ok(users);
    }

    [HttpGet("paginated")]
    public Task<PaginatedList<UserDTO>> GetUsersWithPagination ([FromQuery] GetPaginatedUsersQuery query)
    {
        return _mediator.Send(query);
    }

    [HttpGet("filter")]
    public async Task<ActionResult<List<UserDTO>>> GetUsersWithFilter ([FromQuery] UserFilter filter)
    {
        var query = new GetFilteredUsersQuery { Filter = filter };

        try
        {
            var users = await _mediator.Send(query);

            if (users is null)
            {
                return NotFound("Users was not found.");
            }

            return Ok(users);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Create a new user.
    /// </summary>
    /// <param name="command">The command to create a user.</param>
    /// <returns>The Id of the created user.</returns>
    /// <response code="200">Returns the Id of the newly created user.</response>
    /// <response code="400">If the request is invalid.</response>
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateUser ([FromBody] CreateUserCommand command)
    {
        var userId = await _mediator.Send(command);

        if (userId == Guid.Empty)
        {
            return BadRequest("User was not created.");
        }

        return Ok(userId);
    }

    /// <summary>
    /// Delete a user by Id.
    /// </summary>
    /// <param name="id">The Id of the user to delete.</param>
    /// <response code="204">User successfully deleted.</response>
    /// <response code="404">If the user is not found.</response>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteUser (Guid id)
    {
        var command = new DeleteUserCommand(id);

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

    /// <summary>
    /// Update an existing user.
    /// </summary>
    /// <param name="id">The Id of the user to update.</param>
    /// <param name="command">The command containing updated user data.</param>
    /// <response code="204">User successfully updated.</response>
    /// <response code="400">If the request is invalid or Ids do not match.</response>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUser (Guid id, [FromBody] UpdateUserCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("User Id in URL does not match Id in request body.");
        }

        try
        {
            await _mediator.Send(command);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get all tickets assigned to a specific user.
    /// </summary>
    /// <param name="userId">The Id of the user.</param>
    /// <returns>A list of tickets assigned to the specified user.</returns>
    /// <response code="200">Returns the list of tickets for the user.</response>
    [HttpGet("{userId:guid}/tickets")]
    public async Task<IActionResult> GetTicketsByUserId (Guid userId)
    {
        var query = new GetTicketsByUserIdQuery(userId);
        var result = await _mediator.Send(query);

        return Ok(result);
    }
}
