namespace HackerNews.API.Helpers;

public static class ApiResponseHelper
{
    public static ApiResponse<T> CreateResponse<T>(string message, bool status, T? data)
    {
        return new ApiResponse<T>(message, status, data);
    }

    public static ApiResponse<object> CreateResponse(string message, bool status)
    {
        return new ApiResponse<object>(message, status, null);
    }
}