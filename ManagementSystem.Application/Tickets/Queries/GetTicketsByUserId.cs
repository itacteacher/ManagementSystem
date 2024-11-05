using ManagementSystem.Application.Common.Interfaces;
using ManagementSystem.Application.Tickets.Queries.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.Application.Tickets.Queries;
public record GetTicketsByUserIdQuery : IRequest<List<TicketDTO>>
{
    public Guid UserId { get; set; }

    public GetTicketsByUserIdQuery (Guid userId)
    {
        UserId = userId;
    }
}

public class GetTicketsByUserIdQueryHandler : IRequestHandler<GetTicketsByUserIdQuery, List<TicketDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetTicketsByUserIdQueryHandler (IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TicketDTO>> Handle (GetTicketsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Tickets
            .Where(t => t.UserId == request.UserId)
            .Select(t => new TicketDTO
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                StartDate = t.StartDate,
                DueDate = t.DueDate,
                Status = t.Status
            })
            .ToListAsync(cancellationToken);
    }
}
