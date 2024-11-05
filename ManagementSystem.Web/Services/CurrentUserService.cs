using ManagementSystem.Application.Common.Interfaces;
using System.Security.Claims;

namespace ManagementSystem.Web.Services;

public class CurrentUserService : IUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService (IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetCurrentUser ()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

        return userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId)
            ? userId
            : Guid.NewGuid();
    }
}
