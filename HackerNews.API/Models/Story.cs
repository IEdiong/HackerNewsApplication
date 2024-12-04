namespace HackerNews.API.Models;

public record Story(string by, int descendants, int id, int[]? kids, int score, int time, string title, string type, string url);