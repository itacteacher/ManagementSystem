namespace ManagementSystem.Infrastructure.Emails;
public interface IEmailSender
{
    Task SendEmailAsync (string toEmail, string subject, string body);
}
