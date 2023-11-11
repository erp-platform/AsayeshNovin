using UMS.Authentication.Application.Dtos.UserDtos;

namespace UMS.Authentication.Application.Dtos.VerificationDtos;

public class VerificationResponseDto
{
    public required string Code { get; set; }

    public bool IsVerified { get; set; }

    public Guid? UserId { get; set; }
}