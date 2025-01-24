using ManagementSystem.Domain.Enums;

namespace ManagementSystem.Web.Models;

public class TicketUpdateRequest
{
    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? DueDate { get; set; }

    public Status Status { get; set; }

    public Guid? UserId { get; set; }
}
