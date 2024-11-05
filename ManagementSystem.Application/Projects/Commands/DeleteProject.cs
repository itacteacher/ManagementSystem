using ManagementSystem.Application.Common.Interfaces;
using MediatR;

namespace ManagementSystem.Application.Projects.Commands;

public record DeleteProjectCommand (Guid Id) : IRequest;

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

        if (entity == null)
        {
            throw new Exception($"Entity with Id = {request.Id} was not found");
        }

        _context.Projects.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
