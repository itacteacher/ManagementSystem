using ManagementSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.Application.Users.Queries;

public record GetAllUsersQuery : IRequest<List<UserDTO>>;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDTO>>
{
    private readonly IApplicationDbContext _context;
    public GetAllUsersQueryHandler (IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserDTO>> Handle (GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Users
            .Select(u => new UserDTO
            {
                Id = u.Id,
                FullName = $"{u.FirstName} {u.LastName}",
                Username = u.Username,
                Email = u.Email,
                CreatedAt = u.CreatedAt,
                CreatedBy = u.CreatedBy,
                LastModifiedAt = u.LastModifiedAt,
                LastModifiedBy = u.LastModifiedBy
            }).ToListAsync(cancellationToken);
    }
}
