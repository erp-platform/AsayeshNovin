namespace UMS.Authentication.Application.Dtos;

public class CredentialDto
{
    public required string Username { get; set; }

    public required string Password { get; set; }

    public required Guid UserChannelId { get; set; }
}