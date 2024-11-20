using MediatR;

namespace ManagementSystem.Application.Users.Commands.Delete;

public record DeleteUserCommand : IRequest
{
    public Guid Id { get; set; }

    public DeleteUserCommand (Guid userId)
    {
        Id = userId;
    }
}