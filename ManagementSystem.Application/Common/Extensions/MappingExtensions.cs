using ManagementSystem.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.Application.Common.Extensions;
public static class MappingExtensions
{
    public static Task<PaginatedList<TDestination>> ToPaginatedListAsync<TDestination> (
        this IQueryable<TDestination> queryable, int pageNumber, int pageSize) where TDestination : class
    {
        return PaginatedList<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize);
    }
}
