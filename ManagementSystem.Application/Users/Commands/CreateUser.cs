using FluentValidation;
using ManagementSystem.Application.Common.Interfaces;
using ManagementSystem.Domain.Entities;
using MediatR;

namespace ManagementSystem.Application.Users.Commands;

public record CreateUserCommand : IRequest<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public CreateUserCommandHandler (IApplicationDbContext context, IValidator<CreateUserCommand> validator)
    {
        _context = context;
    }

    public async Task<Guid> Handle (CreateUserCommand request, CancellationToken cancellationToken)
    {
        var entity = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Username = request.Username,
            Email = request.Email
        };

        _context.Users.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}

