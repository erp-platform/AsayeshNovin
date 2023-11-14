namespace UMS.Authentication.Application.Dtos.PasswordResetDtos;

public class PasswordResetResponseDto
{
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public required string Token { get; set; }

    public required bool IsUsed { get; set; }
    public required Guid UserChannelId { get; set; }
}