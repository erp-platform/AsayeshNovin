using UMS.Authentication.Application.Dtos;
using UMS.Authentication.Application.Interfaces;
using UMS.Authentication.Domain.Entities;

namespace api.Controllers.UMS.Authentication;

public class UserController : CrudController<User, UserCreateDto, UserUpdateDto>
{
    public UserController(ICrudService<User, UserCreateDto, UserUpdateDto> authService) : base(authService)
    {
    }
}