using ManagementSystem.Domain.Enums;

namespace ManagementSystem.Infrastructure;
public static class RoleExtensions
{
    public static string ToIdentityRole (this ApplicationRole role)
    {
        return role.ToString();
    }
}
