using Microsoft.AspNetCore.Mvc;
using UMS.Authentication.Application.Dtos;
using UMS.Authentication.Application.Interfaces;
using UMS.Authentication.Domain.Entities;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public class TestController:ControllerBase
{
    private readonly IBaseService<User, UserCreateDto, UserUpdateDto> _userService;

    public TestController(IBaseService<User, UserCreateDto, UserUpdateDto> userService)
    {
        _userService = userService;
    }

    [HttpGet("Db")]
    public virtual async Task<IActionResult> Db()
    {
        return Ok(await _userService.GetAllAsync());
    }
    
    [HttpGet("Ping")]
    public virtual Task<IActionResult> Ping()
    {
        return Task.FromResult<IActionResult>(Ok("Pong"));
    }
}