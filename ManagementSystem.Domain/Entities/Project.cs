using ManagementSystem.Domain.Common;

namespace ManagementSystem.Domain.Entities;

public class Project : BaseAuditableEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public ICollection<Ticket> Tickets { get; set; } = [];
}
