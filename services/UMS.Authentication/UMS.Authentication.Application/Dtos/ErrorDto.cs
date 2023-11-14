namespace UMS.Authentication.Application.Dtos;

public class ErrorDto
{
    public string? Error { get; set; }
    public string? ExceptionMessage { get; set; }
    public string? InnerExceptionMessage { get; set; }
    public string[]? Stacktrace { get; set; }
}