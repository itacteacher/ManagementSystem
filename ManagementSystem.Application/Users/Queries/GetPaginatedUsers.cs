using ManagementSystem.Application.Common.Extensions;
using ManagementSystem.Application.Common.Interfaces;
using ManagementSystem.Application.Common.Models;
using MediatR;

namespace ManagementSystem.Application.Users.Queries;
public record GetPaginatedUsersQuery : IRequest<PaginatedList<UserDTO>>
{
    public int PageSize { get; init; } = 10;
    public int PageNumber { get; init; } = 1;
}

public class GetPaginatedUsersQueryHandler : IRequestHandler<GetPaginatedUsersQuery, PaginatedList<UserDTO>>
{
    private readonly IApplicationDbContext _dbContext;
    public GetPaginatedUsersQueryHandler (IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PaginatedList<UserDTO>> Handle (GetPaginatedUsersQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .Select(u => new UserDTO
            {
                Id = u.Id,
                FullName = $"{u.FirstName} {u.LastName}",
                Username = u.UserName,
                Email = u.Email
            }).ToPaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
