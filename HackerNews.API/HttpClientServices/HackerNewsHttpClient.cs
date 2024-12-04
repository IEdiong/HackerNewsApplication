using System.Text.Json;
using HackerNews.API.Models;

namespace HackerNews.API.HttpClientServices;

public class HackerNewsHttpClient
{
    private readonly HttpClient _httpClient;

    public HackerNewsHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    // Get new stories ids
    public async Task<PaginatedResult<Story>> GetLatestStoriesAsync(int limit, int page)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, _httpClient.BaseAddress + $"/v0/newstories.json");

        var response = await _httpClient.SendAsync(request);
        
        response.EnsureSuccessStatusCode();
        var responseJson = await response.Content.ReadAsStringAsync();
        
        // Deserialize the JSON response into the expected type
        var latestStoriesId = JsonSerializer.Deserialize<int[]>(responseJson);
        
        // Calculate pagination
        int startIndex = (page - 1) * limit;
        int endIndex = startIndex + limit;
        
        // Ensure we don't exceed available stories
        if (startIndex >= latestStoriesId.Length)
        {
            return new PaginatedResult<Story>
            {
                Stories = new List<Story>(),
                Pagination = new PaginationMetadata
                {
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling((double)latestStoriesId.Length / limit),
                    TotalStories = latestStoriesId.Length
                }
            };
        }
        
        // Adjust end index if needed
        endIndex = Math.Min(endIndex, latestStoriesId.Length);


        // Fetch details of first 10 stories
        var stories = new List<Story>();
        for (int i = startIndex; i < endIndex; i++)
        {
            var storyRequest = new HttpRequestMessage(HttpMethod.Get, _httpClient.BaseAddress + $"/v0/item/{latestStoriesId[i]}.json");
            var storyResponse = await _httpClient.SendAsync(storyRequest);

            storyResponse.EnsureSuccessStatusCode();
            var storyResponseJson = await storyResponse.Content.ReadAsStringAsync();
            var story = JsonSerializer.Deserialize<Story>(storyResponseJson);
            
            if (story != null)
            {
                stories.Add(story);
            }
        }
        

        return new PaginatedResult<Story>
        { 
            Stories = stories, 
            Pagination = new PaginationMetadata
            { 
                CurrentPage = page, 
                TotalPages = (int)Math.Ceiling((double)latestStoriesId.Length / limit),
                TotalStories = latestStoriesId.Length
            } 
        };
    }
}