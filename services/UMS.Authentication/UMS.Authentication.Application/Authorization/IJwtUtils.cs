using UMS.Authentication.Domain.Entities;

namespace UMS.Authentication.Application.Authorization;

public interface IJwtUtils
{
    public string GenerateJwtToken(User user);
    public string? ValidateJwtToken(string? token);
}