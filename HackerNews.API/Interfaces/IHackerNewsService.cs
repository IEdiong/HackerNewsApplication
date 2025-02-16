using HackerNews.API.Models;

namespace HackerNews.API.Interfaces;

public interface IHackerNewsService
{
    Task<PaginatedResult<Story>> GetLatestStoriesAsync(int limit, int page, string searchTerm);
}