using Ardalis.GuardClauses;
using ManagementSystem.Application.Common.Interfaces;
using ManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.Application.Tickets.Commands.Create;

public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateTicketCommandHandler (IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle (CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);

        Guard.Against.NotFound(request.ProjectId, project);

        if (request.DueDate.HasValue && request.DueDate.Value >= project.EndDate)
        {
            Guard.Against.OutOfRange(request.DueDate.Value, nameof(request.DueDate), project.StartDate, project.EndDate);
        }

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
