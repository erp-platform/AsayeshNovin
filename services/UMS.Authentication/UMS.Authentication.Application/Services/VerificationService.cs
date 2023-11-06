using Infrastructure.Interfaces;
using UMS.Authentication.Application.Dtos;
using UMS.Authentication.Domain.Entities;

namespace UMS.Authentication.Application.Services;

public class VerificationService : BaseService<Verification, VerificationCreateDto, VerificationUpdateDto>
{
    public VerificationService(IBaseRepository<Verification> repository) : base(repository)
    {
    }

    public override Task<Verification?> CreateAsync(VerificationCreateDto createDto)
    {
        return Repository.CreateAsync(new Verification
        {
            Code = createDto.Code,
            IsVerified = createDto.IsVerified
        });
    }

    public override async Task<Verification?> UpdateAsync(Guid id, VerificationUpdateDto updateDto)
    {
        var verification = await Repository.FindByIdAsync(id);
        if (verification == null)
            throw new Exception($"There's no Verification with id: \"{id}\"");
        if (updateDto.Code != null)
            verification.Code = updateDto.Code;
        verification.IsVerified = updateDto.IsVerified;
        return await Repository.UpdateAsync(verification);
    }
}