namespace UMS.Authentication.Application.Dtos;

public class VerifyDto
{
    /// <example>123456</example>
    public required string Code { get; set; }

    public required Guid UserChannelId { get; set; }
}