using ManagementSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.Application.Projects.Queries;

public record GetAllProjectsQuery : IRequest<List<ProjectDTO>>;

public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, List<ProjectDTO>>
{
    private readonly IApplicationDbContext _context;
    public GetAllProjectsQueryHandler (IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProjectDTO>> Handle (GetAllProjectsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Projects
            .Select(p => new ProjectDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                DurationInDays = (int)(p.EndDate - p.StartDate).TotalDays
            }).ToListAsync(cancellationToken);
    }
}
