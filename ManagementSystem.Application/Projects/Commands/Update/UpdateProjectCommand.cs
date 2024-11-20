using MediatR;

namespace ManagementSystem.Application.Projects.Commands.Update;

public record UpdateProjectCommand : IRequest
{
    public Guid Id { get; init; }

    public string Name { get; init; }

    public string Description { get; init; }

    public DateTime StartDate { get; init; }

    public DateTime EndDate { get; init; }
}
