using ManagementSystem.Application.Tickets.Queries.DTOs;
using MediatR;

namespace ManagementSystem.Application.Tickets.Queries.GetByProjectId;

public record GetTicketsByProjectIdQuery : IRequest<List<TicketDTO>>
{
    public Guid ProjectId { get; set; }

    public GetTicketsByProjectIdQuery (Guid projectId)
    {
        ProjectId = projectId;
    }
}
