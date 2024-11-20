using ManagementSystem.Domain.Common;

namespace ManagementSystem.Domain.Entities;

public class TeamMember : BaseAuditableEntity
{
    public Guid TeamId { get; set; }

    public Team Team { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; }
}
