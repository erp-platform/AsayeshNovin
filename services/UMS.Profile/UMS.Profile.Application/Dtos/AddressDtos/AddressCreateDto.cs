using System.ComponentModel.DataAnnotations;

namespace UMS.Profile.Application.Dtos.AddressDtos;

public class AddressCreateDto
{
    [Required] public required string Text { get; set; }
    public int? PostalCode { get; set; }
    public string? Phone { get; set; }
    [Required] public required string City { get; set; }
}