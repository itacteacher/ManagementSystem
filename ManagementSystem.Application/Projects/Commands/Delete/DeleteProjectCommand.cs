using MediatR;

namespace ManagementSystem.Application.Projects.Commands.Delete;

public record DeleteProjectCommand : IRequest
{
    public Guid Id { get; set; }

    public DeleteProjectCommand (Guid projectId)
    {
        Id = projectId;
    }
}

