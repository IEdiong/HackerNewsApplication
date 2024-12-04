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

    public HackerNewsController(HackerNewsHttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    // GET
    [HttpGet("stories")]
    public async Task<IActionResult> GetListOfNewStories()
    {
        var hackerNewsIds = await _httpClient.GetLatestStoriesAsync();
        return Ok(ApiResponseHelper.CreateResponse("Request Successful", true, hackerNewsIds));
    }
}