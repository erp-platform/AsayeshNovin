namespace UMS.Authentication.Application.Dtos.LoginDtos;

public class LoginResponseDto
{
    /// <example>127.0.0.1</example>
    public required string Ip { get; set; }

    public required bool IsSuccess { get; set; }
}