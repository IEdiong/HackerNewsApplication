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

    public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.WithOrigins("http://127.0.0.1:5500")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin());
        });
}