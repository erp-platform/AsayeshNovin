using Core.Application.Services;
using Core.Infrastructure.Repositories;
using UMS.Authentication.Application.Dtos;
using UMS.Authentication.Application.Dtos.LoginDtos;
using UMS.Authentication.Domain.Entities;

namespace UMS.Authentication.Application.Services;

public class LoginService : BaseService<Login, LoginCreateDto, LoginUpdateDto>
{
    public LoginService(IBaseRepository<Login> repository) : base(repository)
    {
    }

    public override Task<Login?> CreateAsync(LoginCreateDto createDto)
    {
        return Repository.CreateAsync(new Login
        {
            Ip = createDto.Ip,
            IsSuccess = createDto.IsSuccess
        });
    }

    public override async Task<Login?> UpdateAsync(Guid id, LoginUpdateDto updateDto)
    {
        var login = await FindByIdAsync(id);
        if (login == null)
        {
            return null;
        }

        if (updateDto.Ip != null)
        {
            login.Ip = updateDto.Ip;
        }

        login.IsSuccess = updateDto.IsSuccess;
        return await Repository.UpdateAsync(login);
    }
}