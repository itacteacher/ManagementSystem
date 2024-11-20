using ManagementSystem.Domain.Common;
using ManagementSystem.Domain.Enums;

namespace ManagementSystem.Domain.Entities;

public class Ticket : BaseAuditableEntity
{
    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? DueDate { get; set; }

    public Status Status { get; set; }

    public Guid? UserId { get; set; }

    public User? User { get; set; }

    public Guid ProjectId { get; set; }

    public Project Project { get; set; }
}
