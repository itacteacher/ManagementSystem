namespace ManagementSystem.Web.Models;

public class ProjectUpdateRequest
{
    public string Name { get; init; }

    public string Description { get; init; }

    public DateTime StartDate { get; init; }

    public DateTime EndDate { get; init; }
}
