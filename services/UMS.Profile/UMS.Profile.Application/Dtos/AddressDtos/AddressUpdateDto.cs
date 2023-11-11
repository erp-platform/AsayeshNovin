namespace UMS.Profile.Application.Dtos.AddressDtos;

public class AddressUpdateDto
{
    public required string Text { get; set; }
    public int? PostalCode { get; set; }
    public string? Phone { get; set; }
    public required string City { get; set; }
}