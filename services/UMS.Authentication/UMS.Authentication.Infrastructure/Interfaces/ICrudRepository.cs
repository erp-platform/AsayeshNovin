using UMS.Authentication.Domain.Entities;

namespace UMS.Authentication.Infrastructure.Interfaces;

public interface ICrudRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity?>> GetAllAsync();
    Task<TEntity?> CreateAsync(TEntity? entity);
    Task<TEntity?> UpdateAsync(TEntity? entity);
    Task<TEntity?> UpdateOrCreateAsync(TEntity? entity, List<(string, string)> attributes);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> RestoreAsync(Guid id);
    Task<bool> ForceDeleteAsync(Guid id);
}