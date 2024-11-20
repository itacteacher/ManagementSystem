using ManagementSystem.Application.Tickets.Queries.DTOs;
using MediatR;

namespace ManagementSystem.Application.Tickets.Queries.GetById;

public record GetTicketDetailsQuery : IRequest<TicketDetailsDTO>
{
    public Guid Id { get; set; }
}
