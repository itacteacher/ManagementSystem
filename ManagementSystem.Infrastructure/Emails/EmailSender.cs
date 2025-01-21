using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace ManagementSystem.Infrastructure.Emails;
public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender (IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync (string toEmail, string subject, string body)
    {
        var smtpSettings = _configuration.GetSection("SmtpSettings");

        var host = smtpSettings.GetValue<string>("Host");
        var port = smtpSettings.GetValue<int>("Port");
        var username = smtpSettings.GetValue<string>("Username");
        var password = smtpSettings.GetValue<string>("Password");
        var fromAddress = smtpSettings.GetValue<string>("FromAddress");
        var fromName = smtpSettings.GetValue<string>("FromName");

        var client = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(username, password),
            EnableSsl = false
        };

        var message = new MailMessage
        {
            From = new MailAddress(fromAddress, fromName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        message.To.Add(toEmail);

        await client.SendMailAsync(message);
    }
}
