using MediatR;

namespace ManagementSystem.Application.Users.Commands.Create;

public record CreateUserCommand : IRequest<Guid>
{
    public string FirstName { get; init; }

    public string LastName { get; init; }

    public string Username { get; init; }

    public string Email { get; init; }
}

