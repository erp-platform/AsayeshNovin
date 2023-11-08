namespace UMS.Authentication.Application.Dtos;

public class LoginCreateDto
{
    public required string Ip { get; set; }
    public required bool IsSuccess { get; set; }
}