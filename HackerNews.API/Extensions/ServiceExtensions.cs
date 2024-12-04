using HackerNews.API.HttpClientServices;

namespace HackerNews.API.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureHttpClient(this IServiceCollection services, IConfiguration configuration) =>
        services.AddHttpClient<HackerNewsHttpClient>(
            client =>
            {
                var baseUrl = configuration["HackerNewsAPI:BaseUrl"];
                client.BaseAddress = new Uri(baseUrl);
            });
}