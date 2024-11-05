using FluentValidation;
using ManagementSystem.Application.Common.Interfaces;
using ManagementSystem.Domain.Entities;
using MediatR;

namespace ManagementSystem.Application.Projects.Commands;

public record CreateProjectCommand : IRequest<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateProjectCommandHandler (IApplicationDbContext context, IValidator<CreateProjectCommand> validator)
    {
        _context = context;
    }

    public async Task<Guid> Handle (CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var entity = new Project
        {
            Name = request.Name,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };

        _context.Projects.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}

