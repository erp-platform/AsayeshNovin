using UMS.Authentication.Application.Dtos.UserChannelDtos;

namespace UMS.Authentication.Application.Dtos.UserDtos;

public class UserResponseDto
{
    public string? Username { get; set; }
    public Guid? VerificationId { get; set; }

    public virtual IEnumerable<UserChannelResponseDto>? Channels { get; set; } = new List<UserChannelResponseDto>();
}