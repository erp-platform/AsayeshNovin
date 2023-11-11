using Core.Application.Services;
using Core.Infrastructure.Repositories;
using UMS.Authentication.Application.Dtos;
using UMS.Authentication.Application.Dtos.UserChannelDtos;
using UMS.Authentication.Domain.Entities;

namespace UMS.Authentication.Application.Services;

public class UserChannelService : BaseService<UserChannel, UserChannelCreateDto, UserChannelUpdateDto>
{
    public UserChannelService(IBaseRepository<UserChannel> repository) :
        base(repository)
    {
    }

    public override async Task<UserChannel?> CreateAsync(UserChannelCreateDto createDto)
    {
        return await Repository.CreateAsync(new UserChannel
        {
            IsDefault = createDto.IsDefault,
            Value = createDto.Value,
            Channel = createDto.Channel
        });
    }

    public override async Task<UserChannel?> UpdateAsync(Guid id, UserChannelUpdateDto updateDto)
    {
        var userChannel = await Repository.FindByIdAsync(id);
        if (userChannel == null)
            throw new Exception($"There's not UserChannel with id: \"{id}\"");
        userChannel.IsDefault = updateDto.IsDefault;
        if (updateDto.Verification != null)
            userChannel.Verification = updateDto.Verification;
        if (updateDto.Channel != null)
            userChannel.Channel = updateDto.Channel;
        if (updateDto.Value != null)
            userChannel.Value = updateDto.Value;
        if (updateDto.User != null)
            userChannel.User = updateDto.User;
        return await Repository.UpdateAsync(userChannel);
    }
}