using Microsoft.AspNetCore.Mvc;
using Presentation.Errors;
using UMS.Authentication.Application.Dtos;
using UMS.Authentication.Application.Dtos.AuthDtos;
using UMS.Authentication.Application.Dtos.UserChannelDtos;
using UMS.Authentication.Application.Dtos.UserDtos;
using UMS.Authentication.Application.Interfaces;

namespace Presentation.Controllers.UMS.Authentication;

[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
[ExceptionHandler]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthenticationController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Sign up with various Channels (Call/SMS/Email)
    /// </summary>
    [HttpPost("SignUp")]
    [ProducesResponseType(typeof(ResponseDto<UserChannelResponseDto>), 200)]
    [ProducesResponseType(typeof(ResponseDto<string[]>), 400)]
    [ProducesResponseType(typeof(ResponseDto<object?>), 404)]
    [ProducesResponseType(typeof(ResponseDto<object?>), 500)]
    public async Task<IActionResult> SignUp(SignUpDto signUpDto)
    {
        return Ok(await _authService.SignUp(signUpDto));
    }

    /// <summary>
    /// Verify User Channel
    /// </summary>
    [HttpPost("Verify")]
    [ProducesResponseType(typeof(ResponseDto<UserChannelResponseDto>), 200)]
    [ProducesResponseType(typeof(ResponseDto<object?>), 400)]
    [ProducesResponseType(typeof(ResponseDto<object?>), 404)]
    [ProducesResponseType(typeof(ResponseDto<VerifyDto>), 404)]
    public async Task<IActionResult> Verify(VerifyDto verifyDto)
    {
        return Ok(await _authService.Verify(verifyDto));
    }

    /// <summary>
    /// Sets Credential for the provided UserChannel
    /// </summary>
    [HttpPost("SetCredentials")]
    [ProducesResponseType(typeof(ResponseDto<UserResponseDto>), 200)]
    [ProducesResponseType(typeof(ResponseDto<object?>), 400)]
    [ProducesResponseType(typeof(ResponseDto<CredentialDto>), 401)]
    [ProducesResponseType(typeof(ResponseDto<CredentialDto>), 404)]
    [ProducesResponseType(typeof(ResponseDto<CredentialDto>), 500)]
    public async Task<IActionResult> SetCredential(CredentialDto credentialDto)
    {
        var user = (await _authService.SetCredential(credentialDto)).Data;
        return Ok(new
        {
            user?.Username,
            user?.VerificationId
        });
    }

    /// <summary>
    /// Login with Username and Password
    /// </summary>
    [HttpPost("Login")]
    [ProducesResponseType(typeof(ResponseDto<AuthLoginResponseDto>), 200)]
    [ProducesResponseType(typeof(ResponseDto<AuthLoginDto>), 401)]
    public async Task<IActionResult> Login(AuthLoginDto authLoginDto)
    {
        var response = (await _authService.Login(authLoginDto)).Data;
        return Ok(new
        {
            response?.User.Username,
            response?.User.VerificationId,
            response?.Token
        });
    }

    /// <summary>
    /// Password Reset Request
    /// </summary>
    [HttpPost("PasswordReset")]
    [ProducesResponseType(typeof(ResponseDto<PasswordResetRequestDto>), 200)]
    [ProducesResponseType(typeof(ResponseDto<object?>), 501)]
    public async Task<IActionResult> PasswordResetRequest(PasswordResetRequestDto passwordResetRequestDto)
    {
        return Ok(await _authService.PasswordResetRequest(passwordResetRequestDto));
    }

    /// <summary>
    /// Password Reset Action
    /// </summary>
    /// <param name="token">Token that is sent to user using provided channel in the Password Reset Request</param>
    /// <param name="passwordResetAction"></param>
    [HttpPost("PasswordReset/{token}")]
    [ProducesResponseType(typeof(ResponseDto<UserResponseDto>), 200)]
    [ProducesResponseType(typeof(ResponseDto<string>), 404)]
    [ProducesResponseType(typeof(ResponseDto<string>), 500)]
    public async Task<IActionResult> PasswordResetAction(string token, PasswordResetAction passwordResetAction)
    {
        var user = (await _authService.PasswordResetAction(token, passwordResetAction)).Data;
        return Ok(new
        {
            user?.Username,
            user?.VerificationId
        });
    }

    /// <summary>
    /// Login with UserChannel
    /// </summary>
    [ProducesResponseType(typeof(ResponseDto<AuthLoginResponseDto>), 200)]
    [ProducesResponseType(typeof(ResponseDto<object?>), 401)]
    [HttpPost("LoginChannel")]
    public async Task<IActionResult> LoginWithChannel(LoginChannelDto loginChannelDto)
    {
        return Ok(await _authService.LoginWithChannel(loginChannelDto));
    }
}