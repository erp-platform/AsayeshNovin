using Infrastructure.Persistence;
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
    [HttpGet("Db")]
    public virtual async Task<IActionResult> Db()
    {
        return Ok(await new AppDbContext().Database.CanConnectAsync());
    }
    
    [HttpGet("Ping")]
    public virtual Task<IActionResult> Ping()
    {
        return Task.FromResult<IActionResult>(Ok("Pong"));
    }
}