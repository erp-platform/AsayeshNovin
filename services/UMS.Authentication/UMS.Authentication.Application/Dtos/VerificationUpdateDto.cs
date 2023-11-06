namespace UMS.Authentication.Application.Dtos;

public class VerificationUpdateDto
{
    public string? Code { get; set; }

    public bool IsVerified { get; set; }
}