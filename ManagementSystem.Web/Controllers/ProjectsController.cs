using ManagementSystem.Application.Projects.Commands.Create;
using ManagementSystem.Application.Projects.Commands.Delete;
using ManagementSystem.Application.Projects.Commands.Update;
using ManagementSystem.Application.Projects.Queries;
using ManagementSystem.Application.Tickets.Queries.GetByProjectId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ManagementSystem.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectsController (IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all projects.
    /// </summary>
    /// <returns>A list of all projects.</returns>
    /// <response code="200">Returns the list of projects.</response>
    /// <response code="404">If no projects are found.</response>
    [HttpGet]
    public async Task<ActionResult<List<ProjectDTO>>> GetAllProjects ()
    {
        var query = new GetAllProjectsQuery();
        var result = await _mediator.Send(query);

        if (result is null or [])
        {
            return NotFound("Project was not found.");
        }

        return Ok(result);
    }

    /// <summary>
    /// Create a new project.
    /// </summary>
    /// <param name="command">The command to create a project.</param>
    /// <returns>The Id of the created project.</returns>
    /// <response code="200">Returns the Id of the newly created project.</response>
    /// <response code="400">If the request is invalid.</response>
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateProject ([FromBody] CreateProjectCommand command)
    {
        var projectId = await _mediator.Send(command);

        if (projectId == Guid.Empty)
        {
            return BadRequest("Project was not created.");
        }

        return Ok(projectId);
    }

    /// <summary>
    /// Delete a project by Id.
    /// </summary>
    /// <param name="id">The Id of the project.</param>
    /// <response code="204">Project successfully deleted.</response>
    /// <response code="404">If the project is not found.</response>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteProject (Guid id)
    {
        var command = new DeleteProjectCommand(id);

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
    /// Update a project.
    /// </summary>
    /// <param name="id">The Id of the project to update.</param>
    /// <param name="command">The command containing updated project data.</param>
    /// <response code="204">Project successfully updated.</response>
    /// <response code="400">If the request is invalid or Ids do not match.</response>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProject (Guid id, [FromBody] UpdateProjectCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("Project Id in URL does not match Id in request body.");
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
    /// Get all tickets for a specific project.
    /// </summary>
    /// <param name="projectId">The Id of the project to retrieve tickets for.</param>
    /// <returns>A list of tickets associated with the project.</returns>
    /// <response code="200">Returns the list of tickets for the specified project.</response>
    [HttpGet("{projectId:guid}/tickets")]
    public async Task<IActionResult> GetTicketsByProjectId (Guid projectId)
    {
        var query = new GetTicketsByProjectIdQuery(projectId);
        var result = await _mediator.Send(query);

        return Ok(result);
    }
}
