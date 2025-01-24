using Microsoft.Extensions.Configuration;
using System.Xml.Linq;

namespace ManagementSystem.Infrastructure.Emails;
public class EmailTemplateService
{
    private readonly string _templateFilePath;

    public EmailTemplateService (IConfiguration configuration)
    {
        _templateFilePath = configuration["EmailTemplates"]!;
    }

    public EmailTemplate GetTemplateByName (string templateName)
    {
        var xmlDoc = XDocument.Load(_templateFilePath);
        var templateElement = xmlDoc.Descendants("Template")
            .FirstOrDefault(t => t.Attribute("Name")?.Value == templateName);

        if (templateElement == null)
        {
            return null;
        }

        return new EmailTemplate
        {
            TemplateName = templateElement.Attribute("Name")?.Value,
            Subject = templateElement.Element("Subject")?.Value,
            Body = templateElement.Element("Body")?.Value
        };
    }
}
