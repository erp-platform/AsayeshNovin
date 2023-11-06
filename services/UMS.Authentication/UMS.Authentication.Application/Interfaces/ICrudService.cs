using UMS.Authentication.Domain.Entities;

namespace UMS.Authentication.Application.Interfaces;

public interface ICrudService<TEntity, in TCreateDto, in TUpdateDto>
    where TEntity : BaseEntity
    where TCreateDto : class
    where TUpdateDto : class
{
    Task<IEnumerable<TEntity?>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<TEntity?> CreateAsync(TCreateDto createDto);
    Task<TEntity?> UpdateAsync(Guid id, TUpdateDto updateDto);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> RestoreAsync(Guid id);
    Task<bool> ForceDeleteAsync(Guid id);
}