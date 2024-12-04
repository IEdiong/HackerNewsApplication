namespace HackerNews.API.Models;


public class PaginatedResult<T>
{
    public IEnumerable<T> Stories { get; set; }
    public PaginationMetadata Pagination { get; set; }
}