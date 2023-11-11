namespace UMS.Authentication.Application.Dtos.PasswordResetDtos;

public class PasswordResetResponseDto
{
    public required string Token { get; set; }
    public required bool IsUsed { get; set; }
    public required Guid UserChannelId { get; set; }
}