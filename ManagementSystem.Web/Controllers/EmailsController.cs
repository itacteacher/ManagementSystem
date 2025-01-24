using ManagementSystem.Application.Emails;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ManagementSystem.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class EmailsController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmailsController (IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("send-email")]
    public async Task<IActionResult> Send ([FromBody] SendEmailCommand command)
    {
        var emailSent = await _mediator.Send(command);

        if (!emailSent)
        {
            return BadRequest("Email was not sent.");
        }

        return NoContent();
    }
}
