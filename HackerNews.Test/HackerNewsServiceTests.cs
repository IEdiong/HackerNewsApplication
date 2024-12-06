using HackerNews.API.HttpClientServices;
using HackerNews.API.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HackerNews.Test;

public class HackerNewsServiceTests
{
    private Mock<IHackerNewsHttpClient> _mockHttpClient;
    private Mock<IMemoryCache> _mockMemoryCache;
    private Mock<ILogger<HackerNewsHttpClient>> _mockLogger;

    public HackerNewsServiceTests()
    {
        _mockHttpClient = new Mock<IHackerNewsHttpClient>();
        _mockMemoryCache = new Mock<IMemoryCache>();
        _mockLogger = new Mock<ILogger<HackerNewsHttpClient>>();
    }

    [Fact]
    public async Task GetLatestStoriesAsync_ReturnsExpectedResult()
    {
        // Arrange
        var expectedStories = CreateMockStories(50);
        var expectedResult = new PaginatedResult<Story>
        {
            Stories = expectedStories.Take(10).ToList(),
            Pagination = new PaginationMetadata
            {
                CurrentPage = 1,
                TotalPages = 5,
                TotalStories = 50
            }
        };

        // Setup memory cache to simulate caching behavior
        object cachedValue = expectedStories;
        _mockMemoryCache
            .Setup(m => m.TryGetValue(It.IsAny<string>(), out cachedValue))
            .Returns(true);

        // Create HTTP client with mocked dependencies
        var httpClient = new HackerNewsHttpClient(
            new HttpClient(),
            _mockMemoryCache.Object,
            _mockLogger.Object
        );

        // Act
        var result = await httpClient.GetLatestStoriesAsync(10, 1, "");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(10, result.Stories.Count());
        Assert.Equal(5, result.Pagination.TotalPages);
        Assert.Equal(50, result.Pagination.TotalStories);
    }

    [Fact]
    public async Task GetLatestStoriesAsync_VerifyCachingBehavior()
    {
        // Arrange
        var expectedStories = CreateMockStories(500);

        // Setup memory cache to simulate first call (cache miss)
        object cachedValue = null;
        var cacheEntryMock = new Mock<ICacheEntry>();

        _mockMemoryCache
            .Setup(m => m.TryGetValue(It.IsAny<string>(), out cachedValue))
            .Returns(false);

        _mockMemoryCache
            .Setup(m => m.CreateEntry(It.IsAny<object>()))
            .Returns(cacheEntryMock.Object);

        // Create HTTP client with mocked dependencies
        var httpClient = new HackerNewsHttpClient(
            new HttpClient(),
            _mockMemoryCache.Object,
            _mockLogger.Object
        );

        // Act
        // First call should populate the cache
        var firstCallResult = await httpClient.GetLatestStoriesAsync(10, 1, "");

        // Setup cache to return stored stories on subsequent call
        cachedValue = expectedStories;
        _mockMemoryCache
            .Setup(m => m.TryGetValue(It.IsAny<string>(), out cachedValue))
            .Returns(true);

        // Second call should use cached stories
        var secondCallResult = await httpClient.GetLatestStoriesAsync(10, 2, "");

        // Assert
        // Verify cache was set on first call
        _mockMemoryCache.Verify(
            m => m.CreateEntry(It.IsAny<object>()),
            Times.Once
        );

        // Verify second call uses cached stories
        Assert.Equal(10, secondCallResult.Stories.Count());
        Assert.Equal(50, secondCallResult.Pagination.TotalPages);
    }

    // Helper method to create mock stories
    private List<Story> CreateMockStories(int count)
    {
        return Enumerable.Range(1, count)
            .Select(i => new Story("John Doe", 0, i, [], 2, (int)(DateTimeOffset.UtcNow.ToUnixTimeSeconds() - i),
                $"Story {i}", "story", $"https://example.com/story/{i}"
            ))
            .ToList();
    }
}