using Ardalis.GuardClauses;
using ManagementSystem.Application.Common.Interfaces;
using MediatR;

namespace ManagementSystem.Application.Users.Queries;
public record GetUserByIdQuery : IRequest<FullUserDTO>
{
    public Guid UserId { get; set; }

    public GetUserByIdQuery (Guid userId)
    {
        UserId = userId;
    }
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, FullUserDTO>
{
    private readonly IApplicationDbContext _context;

    public GetUserByIdQueryHandler (IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FullUserDTO> Handle (GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(request.UserId);

        Guard.Against.NotFound(request.UserId, user);

        return new FullUserDTO
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }
}
