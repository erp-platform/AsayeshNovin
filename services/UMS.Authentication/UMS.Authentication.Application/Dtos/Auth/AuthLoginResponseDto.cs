using UMS.Authentication.Application.Dtos.UserDtos;

namespace UMS.Authentication.Application.Dtos.Auth;

public class AuthLoginResponseDto
{
    public required UserResponseDto User { get; set; }

    public required string Token { get; set; }
}