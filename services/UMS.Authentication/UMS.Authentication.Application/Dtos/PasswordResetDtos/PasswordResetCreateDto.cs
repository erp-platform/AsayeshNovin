using UMS.Authentication.Domain.Entities;

namespace UMS.Authentication.Application.Dtos.PasswordResetDtos;

public class PasswordResetCreateDto
{
    public required string Token { get; set; }
    public required bool IsUsed { get; set; }
    public UserChannel? UserChannel { get; set; }
}