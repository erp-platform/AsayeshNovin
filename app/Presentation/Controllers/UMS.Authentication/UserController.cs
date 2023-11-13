using Core.Application.Services;
using Core.Presentation.Controllers;
using Presentation.Errors;
using UMS.Authentication.Application.Authorization;
using UMS.Authentication.Application.Dtos.UserChannelDtos;
using UMS.Authentication.Application.Dtos.UserDtos;
using UMS.Authentication.Domain.Entities;

namespace Presentation.Controllers.UMS.Authentication;

[ExceptionHandler]
[Authorize]
public class UserController : BaseController<User, UserCreateDto, UserUpdateDto, UserResponseDto>
{
    private static readonly Func<User?, UserResponseDto> Mapper = user => new UserResponseDto
    {
        Username = user?.Username,
        VerificationId = user?.VerificationId,
        Channels = user?.Channels.Select(c => new UserChannelResponseDto
        {
            Id = c.Id,
            Value = c.Value,
            IsDefault = c.IsDefault,
            UserId = c.User?.Id,
            Channel = c.Channel.AId,
            VerificationId = c.Verification?.Id,
            PasswordResets = null,
            Logins = null
        })
    };

    public UserController(IBaseService<User, UserCreateDto, UserUpdateDto> service) : base(service, Mapper)
    {
    }
}