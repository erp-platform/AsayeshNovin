namespace UMS.Authentication.Application.Dtos.VerificationDtos;

public class VerificationCreateDto
{
    public required string Code { get; set; }

    public bool IsVerified { get; set; } = false;
}