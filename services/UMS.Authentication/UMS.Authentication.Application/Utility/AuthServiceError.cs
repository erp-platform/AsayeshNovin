namespace UMS.Authentication.Application.Utility;

public enum AuthServiceError
{
    NotImplementedError = 101,
    CredentialAlreadySet = 102,
    UserchannelWithoutVerification = 103,
    VerificationCodeIncorrect = 104,
    UserchannelNotFound = 105,
    UserchannelNotVerified = 106,
    DatabaseError = 107,
    UsernamePasswordError = 108,
    PasswordResetTokenError = 109,
    UnknownError = 110,
    GeneralError = 111,
    VerificationCodeExpired = 112,
    ChannelTypeError = 113,
    VerificationAlreadyVerified = 114,
    VerificationIsNotExpired = 115,
}