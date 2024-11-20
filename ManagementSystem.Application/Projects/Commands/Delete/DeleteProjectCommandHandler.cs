using Ardalis.GuardClauses;
using ManagementSystem.Application.Common.Interfaces;
using MediatR;

namespace ManagementSystem.Application.Projects.Commands.Delete;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
{
    private readonly IApplicationDbContext _context;
    public DeleteProjectCommandHandler (IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle (DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Projects.FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _context.Projects.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
