using ManagementSystem.Application.Common.Interfaces;
using ManagementSystem.Domain.Entities;
using MediatR;

namespace ManagementSystem.Application.Tickets.Commands;
public record CreateTicketCommand : IRequest<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }

    public Guid ProjectId { get; set; }
    public Guid? UserId { get; set; }
}

public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateTicketCommandHandler (IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle (CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = new Ticket
        {
            Name = request.Name,
            Description = request.Description,
            StartDate = request.StartDate,
            DueDate = request.DueDate,
            Status = Domain.Enums.Status.NotStarted,
            UserId = request.UserId,
            ProjectId = request.ProjectId
        };

        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync(cancellationToken);

        return ticket.Id;
    }
}
