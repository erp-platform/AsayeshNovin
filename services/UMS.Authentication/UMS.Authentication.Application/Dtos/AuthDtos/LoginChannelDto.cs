namespace UMS.Authentication.Application.Dtos.AuthDtos;

public class LoginChannelDto
{
    /// <example>2</example>
    public required int ChannelId { get; set; }

    /// <example>09338880330</example>
    public required string Value { get; set; }

    public required string Password { get; set; }
}