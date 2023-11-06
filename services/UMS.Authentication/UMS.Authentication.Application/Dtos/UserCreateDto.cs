using System.ComponentModel.DataAnnotations;

namespace UMS.Authentication.Application.Dtos;

public class UserCreateDto
{
    [Required]
    [StringLength(255, MinimumLength = 4)]
    public string? Username { get; set; }

    [Required]
    [MinLength(8)]
    public string? Password { get; set; }

    public Guid VerificationId { get; set; }
}