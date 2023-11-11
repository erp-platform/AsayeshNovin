namespace UMS.Authentication.Application.Dtos.AuthDtos;

public class CredentialDto
{
    /// <example>AnakinSkywalker</example>
    public required string Username { get; set; }

    /// <example>TheChosenOne</example>
    public required string Password { get; set; }

    public required Guid UserChannelId { get; set; }
}