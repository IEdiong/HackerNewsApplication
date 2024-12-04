namespace HackerNews.API.Models;

public class BaseResponse<T>
{
    public string Message { get; set; }
    public string Status { get; set; }
    public T Data { get; set; }
}

public class BaseResponse
{
    public string Message { get; set; }
    public string Status { get; set; }
}