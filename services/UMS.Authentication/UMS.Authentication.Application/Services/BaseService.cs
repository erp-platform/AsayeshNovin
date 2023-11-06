using Infrastructure.Interfaces;
using UMS.Authentication.Application.Interfaces;
using UMS.Authentication.Domain.Entities;

namespace UMS.Authentication.Application.Services;

public abstract class BaseService<TEntity, TCreateDto, TUpdateDto>
    : IBaseService<TEntity, TCreateDto, TUpdateDto>
    where TEntity : BaseEntity
    where TCreateDto : class
    where TUpdateDto : class
{
    protected readonly IBaseRepository<TEntity> Repository;

    protected BaseService(IBaseRepository<TEntity> repository)
    {
        Repository = repository;
    }
    
    public virtual IQueryable<TEntity> GetDbSet()
    {
        return Repository.GetDbSet();
    }

    public virtual async Task<IEnumerable<TEntity?>> GetAllAsync()
    {
        return await Repository.GetAllAsync();
    }

    public virtual async Task<TEntity?> FindByIdAsync(Guid id)
    {
        return await Repository.FindByIdAsync(id);
    }

    public abstract Task<TEntity?> CreateAsync(TCreateDto createDto);

    public abstract Task<TEntity?> UpdateAsync(Guid id, TUpdateDto updateDto);

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await Repository.DeleteAsync(id);
    }

    public async Task<bool> RestoreAsync(Guid id)
    {
        return await Repository.RestoreAsync(id);
    }
    
    public virtual async Task<bool> ForceDeleteAsync(Guid id)
    {
        return await Repository.ForceDeleteAsync(id);
    }
}