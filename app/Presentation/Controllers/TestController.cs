using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using UMS.Authentication.Application.Authorization;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public class TestController : ControllerBase
{
    /// <summary>
    /// Test whether DB can connect or not
    /// </summary>
    /// <returns>true/false</returns>
    [Produces(typeof(bool))]
    [HttpGet("Db")]
    public async Task<IActionResult> Db()
    {
        try
        {
            return Ok(await new AppDbContext().Database.CanConnectAsync());
        }
        catch (Exception e)
        {
            return new JsonResult(new { e.Message, Stacktrace = e.StackTrace })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }

    /// <summary>
    /// This route always returns Pong
    /// </summary>
    /// <returns>Pong</returns>
    [ProducesResponseType(typeof(string), 200)]
    [HttpGet("Ping")]
    public Task<IActionResult> Ping() => Task.FromResult<IActionResult>(Ok("Pong"));

    [Authorize]
    [HttpGet("UserAgent")]
    public JsonResult UserAgent()
    {
        return new JsonResult(new
        {
            IP = HttpContext.Connection.RemoteIpAddress?.ToString(),
            Client = Request.Headers["User-Agent"].ToString()
        });
    }
}