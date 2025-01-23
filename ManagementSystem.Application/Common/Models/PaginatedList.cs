using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.Application.Common.Models;
public class PaginatedList<T>
{
    public IReadOnlyCollection<T> Items { get; }
    public int PageNumber { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }

    public PaginatedList (IReadOnlyCollection<T> items,
        int pageNumber,
        int count,
        int pageSize)
    {
        Items = items;
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
    }

    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public static async Task<PaginatedList<T>> CreateAsync (IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();

        var test = Math.Max(pageNumber, 0);

        var items = await source.Skip(test * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return new PaginatedList<T>(items, pageNumber, count, pageSize);
    }
}
