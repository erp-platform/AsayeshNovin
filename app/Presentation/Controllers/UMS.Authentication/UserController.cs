using UMS.Authentication.Application.Dtos;
using UMS.Authentication.Application.Interfaces;
using UMS.Authentication.Domain.Entities;

namespace Presentation.Controllers.UMS.Authentication;

public class UserController : BaseController<User, UserCreateDto, UserUpdateDto>
{
    public UserController(IBaseService<User, UserCreateDto, UserUpdateDto> service) : base(service)
    {
    }
}