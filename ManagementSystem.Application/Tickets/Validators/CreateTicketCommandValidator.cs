using FluentValidation;
using ManagementSystem.Application.Common.Interfaces;
using ManagementSystem.Application.Tickets.Commands;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.Application.Tickets.Validators;
public class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateTicketCommandValidator (IApplicationDbContext context)
    {
        _context = context;

        RuleFor(t => t.Name)
            .NotEmpty().WithMessage("Ticket name is required.");

        RuleFor(t => t.Description)
            .NotEmpty().WithMessage("Ticket description is required.");

        RuleFor(t => t.StartDate)
            .LessThan(t => t.DueDate).WithMessage("StartDate should be earlier than DueDate.");

        RuleFor(t => t)
            .MustAsync(ProjectExistsAndDueDateIsValid).WithMessage("Project setup is invalid.");

        RuleFor(t => t.UserId)
            .Must(id => id == null || id != Guid.Empty).WithMessage("Valid UserId is required.");
    }

    private async Task<bool> ProjectExistsAndDueDateIsValid (CreateTicketCommand command,
        CancellationToken cancellationToken)
    {
        if (command.ProjectId == Guid.Empty) return false;

        var project = await _context.Projects
                .AsNoTracking()
                .FirstAsync(p => p.Id == command.ProjectId, cancellationToken);

        if (project == null) return false;

        if (command.DueDate.HasValue && command.DueDate.Value >= project.EndDate)
        {
            return false;
        }

        return true;
    }
}
