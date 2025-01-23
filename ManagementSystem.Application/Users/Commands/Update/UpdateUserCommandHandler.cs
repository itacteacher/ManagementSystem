using Ardalis.GuardClauses;
using FluentValidation.Results;
using ManagementSystem.Application.Common.Exceptions;
using ManagementSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.Application.Users.Commands.Update;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserCommandHandler (IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle (UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Users.FindAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        if (entity.Email != request.Email)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email,
                cancellationToken);

            if (existingUser != null)
            {
                var validationFailure = new ValidationFailure("Email", "Email is already taken by another user.");

                throw new ValidationException([validationFailure]);
            }
        }

        entity.FirstName = request.FirstName;
        entity.LastName = request.LastName;
        entity.Email = request.Email;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
