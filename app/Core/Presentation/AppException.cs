namespace Core.Presentation;

public class AppException : Exception
{
    public int? HttpCode { get; set; }

    public string? ErrorText { get; set; }

    public object? ResponseData { get; set; }
}