namespace ManagementSystem.Application.Common.Interfaces;

public interface IEmailSender
{
    Task SendEmailAsync (string recipientEmail, string templateName, Dictionary<string, string> replacements);
}
