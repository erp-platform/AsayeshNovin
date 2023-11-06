using Infrastructure.Interfaces;
using UMS.Authentication.Application.Dtos;
using UMS.Authentication.Domain.Entities;

namespace UMS.Authentication.Application.Services;

public class PasswordResetService : BaseService<PasswordReset, PasswordResetCreateDto, PasswordResetUpdateDto>
{
    public PasswordResetService(IBaseRepository<PasswordReset> repository) : base(repository)
    {
    }

    public override async Task<PasswordReset?> CreateAsync(PasswordResetCreateDto createDto)
    {
        return await Repository.CreateAsync(new PasswordReset
        {
            IsUsed = createDto.IsUsed,
            UserChannel = createDto.UserChannel,
            Token = createDto.Token
        });
    }

    public override async Task<PasswordReset?> UpdateAsync(Guid id, PasswordResetUpdateDto updateDto)
    {
        var passwordReset = await Repository.FindByIdAsync(id);
        if (passwordReset == null)
            throw new Exception($"There's no PasswordReset with id: \"{id}\"");
        if (updateDto.UserChannel != null)
            passwordReset.UserChannel = updateDto.UserChannel;
        if (updateDto.Token != null)
            passwordReset.Token = updateDto.Token;
        passwordReset.IsUsed = updateDto.IsUsed;
        return await Repository.UpdateAsync(passwordReset);
    }
}