using Core.Domain.Entities;

namespace Core.Infrastructure.Repositories;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity?> FindByIdAsync(Guid id);
    Task<IEnumerable<TEntity?>> GetAllAsync();
    IQueryable<TEntity> GetDbSet();
    Task<TEntity?> CreateAsync(TEntity? entity);
    Task<TEntity?> UpdateAsync(TEntity? entity);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> RestoreAsync(Guid id);
    Task<bool> ForceDeleteAsync(Guid id);
}