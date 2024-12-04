using HackerNews.API.Models;

namespace HackerNews.API.Interfaces;

public interface IHackerNewsService
{
    Task<IEnumerable<int[]>> GetNewStoriesIdsAsync();
    Task<IEnumerable<Story>> GetNewStoriesAsync(IEnumerable<int> storiesIds);
}