namespace UMS.Authentication.Application.Dtos;

public class LoginDto
{
    /// <example>ObiWanKenobi</example>
    public required string Username { get; set; }

    /// <example>The Negotiator</example>
    public required string Password { get; set; }
}