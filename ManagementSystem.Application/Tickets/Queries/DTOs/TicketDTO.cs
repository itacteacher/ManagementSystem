using ManagementSystem.Domain.Enums;

namespace ManagementSystem.Application.Tickets.Queries.DTOs;
public class TicketDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public Status Status { get; set; }
}
