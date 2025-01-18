namespace ProductManagement.Application.Models;

public sealed class PaginatedList<T>
{
    public IReadOnlyCollection<T> Items { get; set; }

    public int PageNumber { get; }

    public int PageSize { get; }

    public int TotalCount { get; }

    public int TotalPages { get; }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;

    public bool IsFirstPage => PageNumber == 1;

    public bool IsLastPage => PageNumber == TotalPages;

    private PaginatedList(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize)
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = count;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    }

    public static PaginatedList<T> Create(IEnumerable<T> items, int count, int pageNumber, int pageSize)
    {
        return new PaginatedList<T>(items.ToList(), count, pageNumber, pageSize);
    }
}
