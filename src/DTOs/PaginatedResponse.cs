namespace OrderManagement.DTOs;


public class PaginatedResponse<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public List<T> Items { get; set; } = new();

    public PaginatedResponse() { }

    public PaginatedResponse(List<T> items, int count, int pageNumber, int pageSize)
    {
        Items = items;
        TotalItems = count;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    }
}
