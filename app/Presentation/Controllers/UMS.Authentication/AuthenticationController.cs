using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UMS.Authentication.Application.Dtos;
using UMS.Authentication.Application.Interfaces;
using UMS.Authentication.Domain.Entities;

namespace Presentation.Controllers.UMS.Authentication;

[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
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
    [ProducesResponseType(typeof(UserChannel), 200)]
    [HttpPost("SignUp")]
    public async Task<IActionResult> SignUp(SignUpDto signUpDto)
    {
        return Ok(await _authService.Register(signUpDto));
    }

    /// <summary>
    /// Verify User Channel
    /// </summary>
    [ProducesResponseType(typeof(UserChannel), 200)]
    [HttpPost("Verify")]
    public async Task<IActionResult> Verify(VerifyDto verifyDto)
    {
        return Ok(await _authService.Verify(verifyDto));
    }

    /// <summary>
    /// Sets Credential for the provided UserChannel
    /// </summary>
    [ProducesResponseType(typeof(User), 200)]
    [HttpPost("SetCredentials")]
    public async Task<IActionResult> SetCredential(CredentialDto credentialDto)
    {
        return Ok(await _authService.SetCredential(credentialDto));
    }

    /// <summary>
    /// Login with Username and Password
    /// </summary>
    [ProducesResponseType(typeof(LoginResponseDto), 200)]
    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        return Ok(await _authService.Login(loginDto));
    }

    /// <summary>
    /// Password Reset Request
    /// </summary>
    [ProducesResponseType(typeof(PasswordResetRequestDto), 200)]
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
    [ProducesResponseType(typeof(User), 200)]
    [HttpPost("PasswordReset/{token}")]
    public async Task<IActionResult> PasswordResetAction(string token, PasswordResetAction passwordResetAction)
    {
        return Ok(await _authService.PasswordResetAction(token, passwordResetAction));
    }
}