using UMS.Authentication.Application.Dtos.AuthDtos;
using UMS.Authentication.Application.Dtos.UserChannelDtos;
using UMS.Authentication.Application.Dtos.UserDtos;
namespace UMS.Authentication.Application.Interfaces;

public interface IAuthService
{
    public Task<UserChannelResponseDto> SignUp(SignUpDto signUpDto);

    public Task<UserChannelResponseDto> Verify(VerifyDto verifyDto);

    public Task<UserResponseDto> SetCredential(CredentialDto credentialDto);

    public Task<AuthLoginResponseDto> Login(AuthLoginDto authLoginDto);

    public Task<PasswordResetRequestDto> PasswordResetRequest(PasswordResetRequestDto passwordResetRequestDto);

    public Task<UserResponseDto> PasswordResetAction(string token, PasswordResetAction passwordResetAction);
}