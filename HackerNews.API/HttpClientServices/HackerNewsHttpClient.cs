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
    public async Task<IEnumerable<Story>> GetLatestStoriesAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, _httpClient.BaseAddress + $"/v0/newstories.json");

        var response = await _httpClient.SendAsync(request);
        
        response.EnsureSuccessStatusCode();
        var responseJson = await response.Content.ReadAsStringAsync();
        
        // Deserialize the JSON response into the expected type
        var latestStoriesId = JsonSerializer.Deserialize<int[]>(responseJson);

        // Fetch details of first 10 stories
        var stories = new List<Story>();
        for (int i = 0; i < 10; i++)
        {
            var storyRequest = new HttpRequestMessage(HttpMethod.Get, _httpClient.BaseAddress + $"/v0/item/{latestStoriesId[i]}.json");
            var storyResponse = await _httpClient.SendAsync(storyRequest);

            storyResponse.EnsureSuccessStatusCode();
            var storyResponseJson = await storyResponse.Content.ReadAsStringAsync();
            var story = JsonSerializer.Deserialize<Story>(storyResponseJson);
            stories.Add(story);
        }
        

        return stories;
    }
}