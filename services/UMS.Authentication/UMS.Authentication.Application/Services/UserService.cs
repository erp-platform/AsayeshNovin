using Core.Application.Services;
using Core.Infrastructure.Repositories;
using UMS.Authentication.Application.Dtos;
using UMS.Authentication.Domain.Entities;
using Scrypt;

namespace UMS.Authentication.Application.Services;

public class UserService : BaseService<User, UserCreateDto, UserUpdateDto>
{
    public UserService(IBaseRepository<User> repository) : base(repository)
    {
    }

    public override async Task<User?> CreateAsync(UserCreateDto createDto)
    {
        return await Repository.CreateAsync(new User
        {
            Username = createDto.Username,
            Password = new ScryptEncoder().Encode(createDto.Password),
            VerificationId = createDto.VerificationId
        });
    }

    public override async Task<User?> UpdateAsync(Guid id, UserUpdateDto updateDto)
    {
        var user = await Repository.FindByIdAsync(id);

        if (user == null)
        {
            return null;
        }

        if (!string.IsNullOrEmpty(updateDto.Username))
            user.Username = updateDto.Username;
        if (!string.IsNullOrEmpty(updateDto.Password))
            user.Password = new ScryptEncoder().Encode(updateDto.Password);

        return await Repository.UpdateAsync(user);
    }
}