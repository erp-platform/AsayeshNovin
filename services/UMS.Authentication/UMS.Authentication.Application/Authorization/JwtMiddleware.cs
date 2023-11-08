using Core.Application.Services;
using Microsoft.AspNetCore.Http;
using UMS.Authentication.Application.Dtos;
using UMS.Authentication.Application.Interfaces;
using UMS.Authentication.Domain.Entities;

namespace UMS.Authentication.Application.Authorization;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IBaseService<User, UserCreateDto, UserUpdateDto> userService,
        IJwtUtils jwtUtils)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = jwtUtils.ValidateJwtToken(token);
        if (userId != null)
        {
            // attach user to context on successful jwt validation
            context.Items["User"] = userService.FindByIdAsync(Guid.Parse(userId)).Result;
        }

        await _next(context);
    }
}