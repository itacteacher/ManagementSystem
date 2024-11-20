using MediatR;

namespace ManagementSystem.Application.Projects.Commands.Create;

public record CreateProjectCommand : IRequest<Guid>
{
    public string Name { get; init; }

    public string Description { get; init; }

    public DateTime StartDate { get; init; }

    public DateTime EndDate { get; init; }
}