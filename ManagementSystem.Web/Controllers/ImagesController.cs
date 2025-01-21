using ManagementSystem.Application.Images.Commands;
using ManagementSystem.Application.Images.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ManagementSystem.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ImagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ImagesController (IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{userId}")]
    public async Task<IActionResult> UploadProfileImage (Guid userId, IFormFile image)
    {
        var command = new UploadProfileImageCommand(userId, image);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteProfileImage (Guid userId)
    {
        await _mediator.Send(new DeleteProfileImageCommand(userId));
        return NoContent();
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetProfileImage (Guid userId)
    {
        var result = await _mediator.Send(new GetProfileImageQuery(userId));

        return result;
    }
}
