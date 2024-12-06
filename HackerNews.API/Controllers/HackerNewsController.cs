using HackerNews.API.Helpers;
using HackerNews.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HackerNews.API.Controllers;

[ApiController]
[Route("api/hackernews")]
public class HackerNewsController : ControllerBase
{
    private readonly IHackerNewsService _hackerNewsService;
    private const int DEFAULT_LIMIT = 10;
    private const int MAX_LIMIT = 50;

    public HackerNewsController(IHackerNewsService hackerNewsService)
    {
        _hackerNewsService = hackerNewsService;
    }
    // GET
    [HttpGet("stories")]
    public async Task<IActionResult> GetListOfNewStories(
        [FromQuery] int limit = DEFAULT_LIMIT,
        [FromQuery] int page = 1,
        [FromQuery] string search = "")
    {
        // Validate input parameters
        limit = Math.Clamp(limit, 1, MAX_LIMIT);
        page = Math.Max(1, page);
        
        var latestStories = await _hackerNewsService.GetLatestStoriesAsync(limit, page, search);
        return Ok(ApiResponseHelper.CreateResponse("Request Successful", true, latestStories));
    }
}