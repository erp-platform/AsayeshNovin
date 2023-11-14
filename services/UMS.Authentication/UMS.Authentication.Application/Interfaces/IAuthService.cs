using UMS.Authentication.Application.Dtos;
using UMS.Authentication.Application.Dtos.AuthDtos;
using UMS.Authentication.Application.Dtos.UserChannelDtos;
using UMS.Authentication.Application.Dtos.UserDtos;

namespace UMS.Authentication.Application.Interfaces;

public interface IAuthService
{
    public Task<ResponseDto<UserChannelResponseDto>> SignUp(SignUpDto signUpDto);

    public Task<ResponseDto<UserChannelResponseDto>> Verify(VerifyDto verifyDto);

    public Task<ResponseDto<UserResponseDto>> SetCredential(CredentialDto credentialDto);

    public Task<ResponseDto<AuthLoginResponseDto>> Login(AuthLoginDto authLoginDto);

    public Task<ResponseDto<PasswordResetRequestDto>> PasswordResetRequest(
        PasswordResetRequestDto passwordResetRequestDto);

    public Task<ResponseDto<UserResponseDto>>
        PasswordResetAction(string token, PasswordResetAction passwordResetAction);
}