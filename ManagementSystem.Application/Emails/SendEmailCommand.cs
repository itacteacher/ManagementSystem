using MediatR;

namespace ManagementSystem.Application.Emails;
public record SendEmailCommand (
    string ToEmail,
    string TemplateName,
    Dictionary<string, string> Replacements) : IRequest<bool>;