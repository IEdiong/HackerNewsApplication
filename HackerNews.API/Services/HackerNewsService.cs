using HackerNews.API.HttpClientServices;
using HackerNews.API.Interfaces;
using HackerNews.API.Models;

namespace HackerNews.API.Services;

public class HackerNewsService : IHackerNewsService
{
    private readonly IHackerNewsHttpClient _hackerNewsHttpClient;

    public HackerNewsService(IHackerNewsHttpClient hackerNewsHttpClient)
    {
        _hackerNewsHttpClient = hackerNewsHttpClient;
    }

    public async Task<PaginatedResult<Story>> GetLatestStoriesAsync(int limit, int page, string searchTerm)
    {
        return await _hackerNewsHttpClient.GetLatestStoriesAsync(limit, page, searchTerm);
    }
}