namespace UMS.Authentication.Application.Utility;

public enum AuthServiceError
{
    NotImplementedError,
    CredentialAlreadySet,
    UserChannelWithoutVerification,
    VerificationCodeIncorrect,
    UserChannelNotFound,
    UserChannelNotVerified,
    DatabaseError,
    UsernamePasswordError,
    PasswordResetTokenError,
    UnknownError,
    GeneralError,
    VerificationCodeExpired,
    ChannelTypeError,
    VerificationAlreadyVerified,
    VerificationIsNotExpired
}