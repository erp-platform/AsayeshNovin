namespace UMS.Authentication.Application.Dtos;

public class PasswordResetRequestDto
{
    public required int ChannelId { get; set; }

    public required string Value { get; set; }
}