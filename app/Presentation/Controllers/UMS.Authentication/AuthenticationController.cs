using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Utility;
using UMS.Authentication.Application.Dtos.Auth;
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
    [AllowAnonymous]
    [HttpPost("SignUp")]
    public async Task<IActionResult> SignUp(SignUpDto signUpDto)
    {
        return Ok(await _authService.SignUp(signUpDto));
    }

    /// <summary>
    /// Verify User Channel
    /// </summary>
    [HttpPost("Verify")]
    public async Task<IActionResult> Verify(VerifyDto verifyDto)
    {
        return Ok(await _authService.Verify(verifyDto));
    }

    /// <summary>
    /// Sets Credential for the provided UserChannel
    /// </summary>
    [HttpPost("SetCredentials")]
    public async Task<IActionResult> SetCredential(CredentialDto credentialDto)
    {
        var user = await _authService.SetCredential(credentialDto);
        return Ok(new
        {
            user.Username,
            user.VerificationId
        });
    }

    /// <summary>
    /// Login with Username and Password
    /// </summary>
    [HttpPost("Login")]
    public async Task<IActionResult> Login(AuthLoginDto authLoginDto)
    {
        var response = await _authService.Login(authLoginDto);
        return Ok(new
        {
            response.User.Username,
            response.User.VerificationId,
            response.Token
        });
    }

    /// <summary>
    /// Password Reset Request
    /// </summary>
    [HttpPost("PasswordReset")]
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
    public async Task<IActionResult> PasswordResetAction(string token, PasswordResetAction passwordResetAction)
    {
        var user = await _authService.PasswordResetAction(token, passwordResetAction);
        return Ok(new
        {
            user.Username,
            user.VerificationId
        });
    }
}