namespace UMS.Authentication.Application.Dtos.Auth;

public class AuthLoginDto
{
    /// <example>ObiWanKenobi</example>
    public required string Username { get; set; }

    /// <example>The Negotiator</example>
    public required string Password { get; set; }
}