using ManagementSystem.Domain.Enums;
using MediatR;

namespace ManagementSystem.Application.Tickets.Commands.Update;

public record UpdateTicketCommand : IRequest
{
    public Guid TicketId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? DueDate { get; set; }

    public Status Status { get; set; }

    public Guid? UserId { get; set; }
}
