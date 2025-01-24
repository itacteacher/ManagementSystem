using ManagementSystem.Application.Common.Interfaces;
using MediatR;

namespace ManagementSystem.Application.Emails;

public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, bool>
{
    private readonly IEmailSender _emailSender;

    public SendEmailCommandHandler (IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public async Task<bool> Handle (SendEmailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _emailSender.SendEmailAsync(
                request.ToEmail,
                request.TemplateName,
                request.Replacements);

            return true;
        }
        catch
        {
            return false;
        }
    }
}