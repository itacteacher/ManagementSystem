using Ardalis.GuardClauses;
using ManagementSystem.Application.Common.Interfaces;
using MediatR;

namespace ManagementSystem.Application.Tickets.Commands.Update;

public class UpdateTicketCommandHandler : IRequestHandler<UpdateTicketCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTicketCommandHandler (IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle (UpdateTicketCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Tickets.FindAsync(request.TicketId);

        Guard.Against.NotFound(request.TicketId, entity);

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.StartDate = request.StartDate;
        entity.DueDate = request.DueDate;
        entity.Status = request.Status;
        entity.UserId = request.UserId;

        await _context.SaveChangesAsync(cancellationToken);
    }
}