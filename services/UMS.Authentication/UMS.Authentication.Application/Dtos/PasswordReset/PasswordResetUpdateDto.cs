using UMS.Authentication.Domain.Entities;

namespace UMS.Authentication.Application.Dtos;

public class PasswordResetUpdateDto
{
    public string? Token { get; set; }
    public bool IsUsed { get; set; }
    public UserChannel? UserChannel { get; set; }
}