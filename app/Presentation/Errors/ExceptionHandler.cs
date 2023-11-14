using Core.Presentation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UMS.Authentication.Application.Dtos;

namespace Presentation.Errors;

public class ExceptionHandler : Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var type = context.Exception.GetType();
        if (type == typeof(AppException))
        {
            var exception = (AppException)context.Exception;

            context.Result = new JsonResult(new ResponseDto<object>
            {
                Data = exception.ResponseData,
                Error = new ErrorDto
                {
                    Text = exception.ErrorText,
                    Stacktrace = context.Exception.StackTrace?.Trim().Split("\r\n"),
                    ExceptionMessage = context.Exception.Message.Trim(),
                    InnerExceptionMessage = context.Exception.InnerException?.Message
                }
            }) { StatusCode = exception.HttpCode };
        }
        else
        {
            context.Result = new JsonResult(new ResponseDto<string>
            {
                Data = null,
                Error = new ErrorDto
                {
                    Text = null,
                    Stacktrace = context.Exception.StackTrace?.Trim().Split("\r\n"),
                    ExceptionMessage = context.Exception.Message.Trim(),
                    InnerExceptionMessage = context.Exception.InnerException?.Message
                }
            }) { StatusCode = StatusCodes.Status400BadRequest };
        }
    }
}