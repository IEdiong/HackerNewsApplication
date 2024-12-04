namespace HackerNews.API.Models;

public class PaginationMetadata
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalStories { get; set; }
}