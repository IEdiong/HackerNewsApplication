namespace HackerNews.API.Helpers;

public class ApiResponse<T>
{
    public string Message { get; set; }
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }

    public ApiResponse(string message, bool isSuccess, T? data)
    {
        Message = message;
        IsSuccess = isSuccess;
        Data = data;
    }
}