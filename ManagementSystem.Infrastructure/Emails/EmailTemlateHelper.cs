namespace ManagementSystem.Infrastructure.Emails;
public static class EmailTemplateHelper
{
    public static string PopulateTemplate (string templateBody, Dictionary<string, string> replacements)
    {
        foreach (var replacement in replacements)
        {
            templateBody = templateBody.Replace($"{{{{{replacement.Key}}}}}", replacement.Value);
        }

        return templateBody;
    }
}
