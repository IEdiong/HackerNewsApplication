using Microsoft.AspNetCore.Mvc;

namespace HackerNews.API.Controllers;

[ApiController]
[Route("api/hackernews")]
public class HackerNewsController : ControllerBase
{
    // GET
    public IEnumerable<Object> GetListOfNewStories()
    {
        return new List<Object>();
    }
}