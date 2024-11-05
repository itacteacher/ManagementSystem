using FluentValidation;
using ManagementSystem.Application.Common.Interfaces;
using ManagementSystem.Application.Users.Commands;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.Application.Users.Validators;
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly IApplicationDbContext _context;
    public CreateUserCommandValidator (IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(5).WithMessage("Username must be at least 5 characters long.")
            .MaximumLength(20).WithMessage("Username cannot exceed 20 characters.")
            .MustAsync(BeUniqueUsername).WithMessage("Username must be unique.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email should have a valid email format.");
    }

    private async Task<bool> BeUniqueUsername (string username, CancellationToken cancellationToken)
    {
        return !await _context.Users.AnyAsync(u => u.Username == username, cancellationToken);
    }
}
