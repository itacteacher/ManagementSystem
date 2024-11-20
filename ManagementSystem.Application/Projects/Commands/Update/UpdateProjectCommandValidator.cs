using FluentValidation;

namespace ManagementSystem.Application.Projects.Commands.Update;

public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator ()
    {
        RuleFor(p => p.Name)
            .MaximumLength(50)
            .NotEmpty()
            .WithMessage("Name is required.");

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
}
