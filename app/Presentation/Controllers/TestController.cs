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
    private class Error
    {
        public string? Message { get; set; }
        public string? Stacktrace { get; set; }
    }

    /// <summary>
    /// Test whether DB can connect or not
    /// </summary>
    /// <returns>true/false</returns>
    [Produces(typeof(bool))]
    [ProducesResponseType(typeof(Error), 500)]
    [HttpGet("Db")]
    public async Task<IActionResult> Db()
    {
        try
        {
            return Ok(await new AppDbContext().Database.CanConnectAsync());
        }
        catch (Exception e)
        {
            return new JsonResult(new Error { Message = e.Message, Stacktrace = e.StackTrace })
            {
                StatusCode = 500
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