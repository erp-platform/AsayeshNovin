using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.Utility;

public class ExceptionHandler : Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        context.Result = new JsonResult(new
        {
            Message = context.Exception.Message.Trim(),
            Stacktrace = context.Exception.StackTrace?.Trim().Split("\r\n"),
            InnerException = context.Exception.InnerException?.Message
        }) { StatusCode = StatusCodes.Status400BadRequest };
    }
}