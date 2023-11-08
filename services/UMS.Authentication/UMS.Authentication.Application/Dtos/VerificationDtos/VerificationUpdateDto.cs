namespace UMS.Authentication.Application.Dtos.VerificationDtos;

public class VerificationUpdateDto
{
    public string? Code { get; set; }

    public bool IsVerified { get; set; }
}