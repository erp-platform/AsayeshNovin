using Core.Application.Services;
using Core.Presentation.Controllers;
using Presentation.Utility;
using UMS.Authentication.Application.Authorization;
using UMS.Authentication.Application.Dtos;
using UMS.Authentication.Domain.Entities;

namespace Presentation.Controllers.UMS.Authentication;

[ExceptionHandler]
[Authorize]
public class UserController : BaseController<User, UserCreateDto, UserUpdateDto>
{
    public UserController(IBaseService<User, UserCreateDto, UserUpdateDto> service) : base(service)
    {
    }
}