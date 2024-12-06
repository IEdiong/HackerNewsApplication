using HackerNews.API.Models;

namespace HackerNews.API.HttpClientServices;

public interface IHackerNewsHttpClient
{
    Task<PaginatedResult<Story>> GetLatestStoriesAsync(int limit, int page, string searchTerm);
}