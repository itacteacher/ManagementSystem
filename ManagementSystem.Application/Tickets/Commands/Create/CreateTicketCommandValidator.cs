using FluentValidation;

namespace ManagementSystem.Application.Tickets.Commands.Create;

public class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
{
    public CreateTicketCommandValidator ()
    {
        RuleFor(t => t.Name)
            .NotEmpty().WithMessage("Ticket name is required.");

        RuleFor(t => t.Description)
            .NotEmpty().WithMessage("Ticket description is required.");

        RuleFor(t => t.StartDate)
            .LessThan(t => t.DueDate).WithMessage("StartDate should be earlier than DueDate.");

        RuleFor(t => t.ProjectId)
            .NotEmpty().WithMessage("Project Id is required.");

        RuleFor(t => t.UserId)
            .Must(id => id == null || id != Guid.Empty).WithMessage("Valid UserId is required.");
    }
}
