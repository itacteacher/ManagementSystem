using ManagementSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.Application.Users.Queries;
public record GetFilteredUsersQuery : IRequest<List<UserDTO>>
{
    public UserFilter? Filter { get; set; }
}

public class GetFilteredUsersQueryHandler : IRequestHandler<GetFilteredUsersQuery, List<UserDTO>>
{
    private readonly IApplicationDbContext _dbContext;
    public GetFilteredUsersQueryHandler (IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<UserDTO>> Handle (GetFilteredUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Users.AsNoTracking();

        if (request.Filter != null)
        {
            if (!string.IsNullOrWhiteSpace(request.Filter.UserName))
            {
                query = query.Where(u => u.UserName.Contains(request.Filter.UserName));
            }

            if (!string.IsNullOrWhiteSpace(request.Filter.Email))
            {
                query = query.Where(u => u.Email.Contains(request.Filter.Email));
            }

            if (!string.IsNullOrWhiteSpace(request.Filter.LastName))
            {
                query = query.Where(u => u.LastName.Contains(request.Filter.LastName));
            }
        }

        var users = await query.Select(u => new UserDTO
        {
            Id = u.Id,
            FullName = $"{u.FirstName} {u.LastName}",
            Username = u.UserName,
            Email = u.Email
        }).ToListAsync();

        return users;
    }
}
