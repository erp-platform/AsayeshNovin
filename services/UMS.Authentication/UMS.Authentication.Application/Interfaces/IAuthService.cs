using UMS.Authentication.Application.Dtos;
using UMS.Authentication.Application.Dtos.Auth;
using UMS.Authentication.Application.Dtos.UserChannelDtos;
using UMS.Authentication.Application.Dtos.UserDtos;
using UMS.Authentication.Domain.Entities;

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