using MediatR;

namespace ManagementSystem.Application.Users.Commands.Update;

public record UpdateUserCommand : IRequest
{
    public Guid Id { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public string Email { get; init; }
}
