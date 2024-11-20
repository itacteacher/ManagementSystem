using ManagementSystem.Domain.Common;

namespace ManagementSystem.Domain.Entities;

public class Team : BaseAuditableEntity
{
    public string Name { get; set; }

    public Guid ProjectId { get; set; }

    public Project Project { get; set; }

    public ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
}
