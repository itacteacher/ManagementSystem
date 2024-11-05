using ManagementSystem.Application.Common.Interfaces;
using MediatR;

namespace ManagementSystem.Application.Projects.Commands;

public record UpdateProjectCommand : IRequest
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
}

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProjectCommandHandler (IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle (UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Projects.FindAsync([request.Id], cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity with Id = {request.Id} was not found.");
        }

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.StartDate = request.StartDate;
        entity.EndDate = request.EndDate;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
