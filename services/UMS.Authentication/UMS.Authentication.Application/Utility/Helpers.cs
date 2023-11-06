namespace UMS.Authentication.Application.Utility;

public static class Helpers
{
    private static Random _random = new();

    public static string GenerateVerificationCode() => _random.Next(100000, 999999).ToString();

    public static string GeneratePasswordResetToken() => Guid.NewGuid().ToString();
}