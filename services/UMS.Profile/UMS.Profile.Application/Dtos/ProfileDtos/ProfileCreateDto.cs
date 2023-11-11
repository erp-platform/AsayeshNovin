using System.ComponentModel.DataAnnotations;

namespace UMS.Profile.Application.Dtos.ProfileDtos;

public class ProfileCreateDto
{
    [Required] public required string Forename { get; set; }
    [Required] public required string Surname { get; set; }
    public string? MiddleName { get; set; }
    public string? NationalCode { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? FatherName { get; set; }
}