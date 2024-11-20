using MediatR;

namespace ManagementSystem.Application.Tickets.Commands.Create;

public record CreateTicketCommand : IRequest<Guid>
{
    public string Name { get; init; }

    public string Description { get; init; }

    public DateTime? StartDate { get; init; }

    public DateTime? DueDate { get; init; }

    public Guid ProjectId { get; init; }

    public Guid? UserId { get; init; }
}
