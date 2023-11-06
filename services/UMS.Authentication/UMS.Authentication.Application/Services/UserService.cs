using UMS.Authentication.Application.Dtos;
using UMS.Authentication.Domain.Entities;
using UMS.Authentication.Infrastructure.Interfaces;
using Scrypt;

namespace UMS.Authentication.Application.Services;

public class UserService : CrudService<User, UserCreateDto, UserUpdateDto>
{
    public UserService(ICrudRepository<User> repository) : base(repository)
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
        var user = await Repository.GetByIdAsync(id);

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