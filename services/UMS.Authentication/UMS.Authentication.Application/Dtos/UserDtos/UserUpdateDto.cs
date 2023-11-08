using System.ComponentModel.DataAnnotations;

namespace UMS.Authentication.Application.Dtos.UserDtos;

public class UserUpdateDto
{
    [StringLength(255, MinimumLength = 4)]
    public string? Username { get; set; }

    [MinLength(8)]
    public string? Password { get; set; }
}