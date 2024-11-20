using FluentValidation;
using ManagementSystem.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.Application.Projects.Commands.Create;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateProjectCommandValidator (IApplicationDbContext context)
    {
        _context = context;

        RuleFor(p => p.Name)
            .MaximumLength(50)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MustAsync(BeUniqueName).WithMessage("Project name should be unique.");

        RuleFor(p => p.Description)
            .MaximumLength(200)
            .NotEmpty()
            .WithMessage("Description is required.");

        RuleFor(p => p.StartDate)
            .LessThan(p => p.EndDate)
            .WithMessage("Start date must be earlier than end date.")
            .NotEmpty()
            .WithMessage("Start date is required.");
    }

    private async Task<bool> BeUniqueName (string name, CancellationToken cancellationToken)
    {
        return !await _context.Projects.AnyAsync(p => p.Name == name, cancellationToken);
    }
}
