using System.Collections.Concurrent;
using System.Text.Json;
using HackerNews.API.Models;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNews.API.HttpClientServices;

public class HackerNewsHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _memoryCache;
    private readonly SemaphoreSlim _semaphore = new(1,1);
    private readonly string cacheKey = "hackerNewsCacheKey";
    private readonly ILogger<HackerNewsHttpClient> _logger;

    public HackerNewsHttpClient(
        HttpClient httpClient,
        IMemoryCache memoryCache,
        ILogger<HackerNewsHttpClient> logger)
    {
        _httpClient = httpClient;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    // Get latest stories
    public async Task<PaginatedResult<Story>> GetLatestStoriesAsync(int limit, int page)
    {
        // Try to get cached stories first
        if (_memoryCache.TryGetValue(cacheKey, out List<Story>? cachedStories) && cachedStories != null)
        {
            return PaginateStories(cachedStories, limit, page);
        }

        // Use semaphore to ensure only one thread refreshes the cache
        await _semaphore.WaitAsync();
        try 
        {
            // Double-check locking pattern
            if (!_memoryCache.TryGetValue(cacheKey, out cachedStories))
            {
                cachedStories = await FetchAndCacheAllStoriesAsync();
            }

            return PaginateStories(cachedStories, limit, page);
        }
        finally 
        {
            _semaphore.Release();
        }
    }

    private async Task<List<Story>> FetchAndCacheAllStoriesAsync()
    {
        try 
        {
            // Fetch story IDs
            var request = new HttpRequestMessage(HttpMethod.Get, _httpClient.BaseAddress + $"/v0/newstories.json");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync();

            // Deserialize story IDs
            var latestStoriesId = JsonSerializer.Deserialize<int[]>(responseJson);

            // Fetch full story details concurrently
            var stories = new ConcurrentBag<Story>();
            
            await Parallel.ForEachAsync(
                latestStoriesId.Take(500), 
                new ParallelOptions { MaxDegreeOfParallelism = 10 }, 
                async (id, token) =>
                {
                    try 
                    {
                        var storyRequest = new HttpRequestMessage(HttpMethod.Get, 
                            _httpClient.BaseAddress + $"/v0/item/{id}.json");
                        var storyResponse = await _httpClient.SendAsync(storyRequest, token);
                        
                        storyResponse.EnsureSuccessStatusCode();
                        var storyResponseJson = await storyResponse.Content.ReadAsStringAsync();
                        var story = JsonSerializer.Deserialize<Story>(storyResponseJson);
                        
                        if (story != null)
                        {
                            stories.Add(story);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error fetching story with ID {id}");
                    }
                });

            // Convert to list and sort (optional, based on your requirements)
            var storyList = stories
                .OrderByDescending(s => s.Time)
                .ToList();

            // Cache the stories
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                .SetAbsoluteExpiration(TimeSpan.FromHours(2))
                .SetPriority(CacheItemPriority.High);
            
            _memoryCache.Set(cacheKey, storyList, cacheOptions);

            return storyList;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching and caching Hacker News stories");
            return new List<Story>();
        }
    }

    private PaginatedResult<Story> PaginateStories(List<Story> stories, int limit, int page)
    {
        // Calculate pagination
        int startIndex = (page - 1) * limit;
        int endIndex = Math.Min(startIndex + limit, stories.Count);
        
        var paginatedStories = stories
            .Skip(startIndex)
            .Take(limit)
            .ToList();

        return new PaginatedResult<Story>
        {
            Stories = paginatedStories,
            Pagination = new PaginationMetadata
            {
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)stories.Count / limit),
                TotalStories = stories.Count
            }
        };
    }
}