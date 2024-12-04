using HackerNews.API.Interfaces;
using HackerNews.API.Models;

namespace HackerNews.API.Services;

public class HackerNewsService : IHackerNewsService
{
    public Task<IEnumerable<int[]>> GetNewStoriesIdsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Story>> GetNewStoriesAsync(IEnumerable<int> storiesIds)
    {
        throw new NotImplementedException();
    }
}