namespace ECommerceAPI.Application.Shared;

public class Pagination<T> where T : class
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int Count { get; set; }
    public IReadOnlyList<T> Data { get; set; } = Array.Empty<T>();
}
