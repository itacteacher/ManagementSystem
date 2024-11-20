using ManagementSystem.Application.Tickets.Queries.DTOs;
using MediatR;

namespace ManagementSystem.Application.Tickets.Queries.GetByUserId;

public record GetTicketsByUserIdQuery : IRequest<List<TicketDTO>>
{
    public Guid UserId { get; set; }

    public GetTicketsByUserIdQuery (Guid userId)
    {
        UserId = userId;
    }
}
