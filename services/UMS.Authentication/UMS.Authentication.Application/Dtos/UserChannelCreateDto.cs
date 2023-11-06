using UMS.Authentication.Domain.Entities;

namespace UMS.Authentication.Application.Dtos;

public class UserChannelCreateDto
{
    public required string Value { get; set; }
    public required Channel Channel { get; set; }

    public bool IsDefault { get; set; }
}