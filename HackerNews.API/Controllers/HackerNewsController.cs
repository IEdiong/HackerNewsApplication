using HackerNews.API.Helpers;
using HackerNews.API.HttpClientServices;
using HackerNews.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HackerNews.API.Controllers;

[ApiController]
[Route("api/hackernews")]
public class HackerNewsController : ControllerBase
{
    private readonly HackerNewsHttpClient _httpClient;
    private const int DEFAULT_LIMIT = 10;
    private const int MAX_LIMIT = 50;

    public HackerNewsController(HackerNewsHttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    // GET
    [HttpGet("stories")]
    public async Task<IActionResult> GetListOfNewStories([FromQuery] int limit = DEFAULT_LIMIT, [FromQuery] int page = 1)
    {
        // Validate input parameters
        limit = Math.Clamp(limit, 1, MAX_LIMIT);
        page = Math.Max(1, page);
        
        var hackerNewsIds = await _httpClient.GetLatestStoriesAsync(limit, page);
        return Ok(ApiResponseHelper.CreateResponse("Request Successful", true, hackerNewsIds));
    }
}