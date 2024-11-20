using Ardalis.GuardClauses;
using ManagementSystem.Application.Common.Interfaces;
using MediatR;

namespace ManagementSystem.Application.Users.Commands.Delete;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IApplicationDbContext _context;
    public DeleteUserCommandHandler (IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle (DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Users.FindAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _context.Users.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
