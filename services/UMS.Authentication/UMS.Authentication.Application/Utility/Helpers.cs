namespace UMS.Authentication.Application.Utility;

public static class Helpers
{
    private static readonly Random Random = new();

    public static string GenerateVerificationCode() => Random.Next(100000, 999999).ToString();

    public static string GeneratePasswordResetToken() => Guid.NewGuid().ToString();
}