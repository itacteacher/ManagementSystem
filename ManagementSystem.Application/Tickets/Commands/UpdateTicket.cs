using ManagementSystem.Application.Common.Interfaces;
using ManagementSystem.Domain.Enums;
using MediatR;

namespace ManagementSystem.Application.Tickets.Commands;
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

public class UpdateTicketCommandHandler : IRequestHandler<UpdateTicketCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTicketCommandHandler (IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle (UpdateTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _context.Tickets.FindAsync(request.TicketId);

        if (ticket == null)
        {
            throw new Exception($"Entity with Id = {request.TicketId} was not found.");
        }

        ticket.Name = request.Name;
        ticket.Description = request.Description;
        ticket.StartDate = request.StartDate;
        ticket.DueDate = request.DueDate;
        ticket.Status = request.Status;
        ticket.UserId = request.UserId;

        await _context.SaveChangesAsync(cancellationToken);
    }
}