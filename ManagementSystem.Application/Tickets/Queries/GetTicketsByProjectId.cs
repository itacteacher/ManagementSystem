using ManagementSystem.Application.Common.Interfaces;
using ManagementSystem.Application.Tickets.Queries.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.Application.Tickets.Queries;
public record GetTicketsByProjectIdQuery : IRequest<List<TicketDTO>>
{
    public Guid ProjectId { get; set; }

    public GetTicketsByProjectIdQuery (Guid projectId)
    {
        ProjectId = projectId;
    }
}

public class GetTicketsByProjectIdQueryHandler : IRequestHandler<GetTicketsByProjectIdQuery, List<TicketDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetTicketsByProjectIdQueryHandler (IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TicketDTO>> Handle (GetTicketsByProjectIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Tickets
            .Where(t => t.ProjectId == request.ProjectId)
            .Select(t => new TicketDTO
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                StartDate = t.StartDate,
                DueDate = t.DueDate,
                Status = t.Status
            }).ToListAsync(cancellationToken);
    }
}
