using Core.Presentation;

namespace UMS.Authentication.Application.Utility;

public static class Helpers
{
    private static readonly Random Random = new();

    public static string GenerateVerificationCode() => Random.Next(100000, 999999).ToString();

    public static string GeneratePasswordResetToken() => Guid.NewGuid().ToString();

    public static AppException CreateAuthException(AuthServiceError error, object? data = null, int httpCode = 400)
    {
        return new AppException
        {
            ErrorCode = (int)(object)error,
            ErrorText = error.ToString(),
            HttpCode = httpCode,
            ResponseData = data
        };
    }
}