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

    [AllowAnonymous]
    [ProducesResponseType(typeof(UserChannel), 200)]
    [HttpPost("SignUp")]
    public async Task<IActionResult> SignUp(SignUpDto signUpDto)
    {
        return Ok(await _authService.Register(signUpDto));
    }

    [ProducesResponseType(typeof(UserChannel), 200)]
    [HttpPost("Verify")]
    public async Task<IActionResult> Verify(VerifyDto verifyDto)
    {
        return Ok(await _authService.Verify(verifyDto));
    }

    /// <summary>
    /// Sets Credential for the provided UserChannel
    /// </summary>
    /// <param name="credentialDto"></param>
    /// <returns>User</returns>
    [ProducesResponseType(typeof(User), 200)]
    [HttpPost("SetCredentials")]
    public async Task<IActionResult> SetCredential(CredentialDto credentialDto)
    {
        return Ok(await _authService.SetCredential(credentialDto));
    }

    /// <summary>
    /// Login with Username and Password
    /// </summary>
    /// <param name="loginDto"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(LoginResponseDto), 200)]
    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        return Ok(await _authService.Login(loginDto));
    }

    [ProducesResponseType(typeof(PasswordResetRequestDto), 200)]
    [HttpPost("PasswordReset")]
    public async Task<IActionResult> PasswordResetRequest(PasswordResetRequestDto passwordResetRequestDto)
    {
        return Ok(await _authService.PasswordResetRequest(passwordResetRequestDto));
    }

    [ProducesResponseType(typeof(User), 200)]
    [HttpPost("PasswordReset/{token}")]
    public async Task<IActionResult> PasswordResetAction(string token, PasswordResetAction passwordResetAction)
    {
        return Ok(await _authService.PasswordResetAction(token, passwordResetAction));
    }
}