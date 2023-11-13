using UMS.Authentication.Application.Dtos.LoginDtos;
using UMS.Authentication.Application.Dtos.PasswordResetDtos;

namespace UMS.Authentication.Application.Dtos.UserChannelDtos;

public class UserChannelResponseDto
{
    public required Guid Id { get; set; }
    public required string Value { get; set; }
    public required bool IsDefault { get; set; }
    public Guid? UserId { get; set; }
    public required int Channel { get; set; }
    public Guid? VerificationId { get; set; }

    public virtual IEnumerable<PasswordResetResponseDto>? PasswordResets { get; set; } =
        new List<PasswordResetResponseDto>();

    public virtual IEnumerable<LoginResponseDto>? Logins { get; set; } = new List<LoginResponseDto>();
}