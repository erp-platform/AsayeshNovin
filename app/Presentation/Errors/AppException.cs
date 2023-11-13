namespace Presentation.Errors;

public class AppException : Exception
{
    public required int ErrorCode { get; init; }

    public string? ErrorText { get; set; }
}