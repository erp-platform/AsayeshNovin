using UMS.Authentication.Domain.Entities;

namespace UMS.Authentication.Application.Dtos;

public class LoginResponseDto
{
    public required User User { get; set; }

    public required string Token { get; set; }
}