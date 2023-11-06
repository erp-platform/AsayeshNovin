namespace UMS.Authentication.Application.Dtos;

public class VerifyDto
{
    public required string Code { get; set; }

    public required Guid UserChannelId { get; set; }
}