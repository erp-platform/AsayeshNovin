using System.ComponentModel.DataAnnotations;

namespace UMS.Authentication.Application.Dtos.UserDtos;

public class UserCreateDto
{
    [Required]
    [StringLength(255, MinimumLength = 4)]
    public required string Username { get; set; }

    [Required] [MinLength(8)] public required string Password { get; set; }

    public required Guid VerificationId { get; set; }
}