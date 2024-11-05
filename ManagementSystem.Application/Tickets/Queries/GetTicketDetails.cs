using ManagementSystem.Application.Common.Interfaces;
using ManagementSystem.Application.Tickets.Queries.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.Application.Tickets.Queries;
public record GetTicketDetailsQuery : IRequest<TicketDetailsDTO>
{
    public Guid Id { get; set; }
}

public class GetTicketDetailsQueryHandler : IRequestHandler<GetTicketDetailsQuery, TicketDetailsDTO>
{
    private readonly IApplicationDbContext _context;

    public GetTicketDetailsQueryHandler (IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TicketDetailsDTO> Handle (GetTicketDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var ticketDetails = await _context.Tickets
            .Where(t => t.Id == request.Id)
            .Select(t => new TicketDetailsDTO
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                StartDate = t.StartDate,
                DueDate = t.DueDate,
                Status = t.Status,
                Username = t.User != null ? t.User.Username : string.Empty,
                ProjectName = t.Project.Name
            }).FirstOrDefaultAsync(cancellationToken);

        if (ticketDetails == null)
        {
            throw new Exception($"Ticket with Id {request.Id} was not found.");
        }

        return ticketDetails;
    }
}
