using Core.Presentation;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Presentation.Errors;
using UMS.Authentication.Application.Authorization;
using UMS.Authentication.Application.Dtos;

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

    [HttpGet("UserAgent")]
    public JsonResult UserAgent()
    {
        return new JsonResult(new
        {
            IP = HttpContext.Connection.RemoteIpAddress?.ToString(),
            Client = Request.Headers["User-Agent"].ToString(),
            DateTime.UtcNow,
            DateTime.Now
        });
    }

    [Authorize]
    [ExceptionHandler]
    [ProducesResponseType(typeof(ResponseDto<string>), 400)]
    [HttpGet("Error")]
    public JsonResult ErrorTest()
    {
        var number = new Random().Next(1, 5);
        const string text = "Error number: {number}";
        throw number switch
        {
            1 => new AppException { ErrorText = number.ToString() },
            2 => new ArgumentException(text),
            3 => new ArithmeticException(text),
            4 => new ApplicationException(text),
            _ => new FormatException(text)
        };
    }
}