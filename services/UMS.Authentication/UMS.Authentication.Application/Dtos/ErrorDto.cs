namespace UMS.Authentication.Application.Dtos;

public class ErrorDto
{
    public required int ErrorCode { get; set; }

    public string? ErrorText { get; set; }

    public string[]? Stacktrace { get; set; }

    public string? ExceptionMessage { get; set; }

    public string? InnerExceptionMessage { get; set; }
}