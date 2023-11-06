using System.Text;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using UMS.Authentication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    private readonly AppDbContext _context;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public virtual IQueryable<TEntity> GetDbSet()
    {
        return _context.Set<TEntity>();
    }

    public virtual async Task<TEntity?> FindByIdAsync(Guid id)
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public virtual async Task<IEnumerable<TEntity?>> GetAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public virtual async Task<TEntity?> CreateAsync(TEntity? entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _context.Set<TEntity>().Add(entity);

        _context.Entry(entity).State = EntityState.Added;

        return await SaveAsyncChanges(entity);
    }

    public virtual async Task<TEntity?> UpdateAsync(TEntity? entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _context.Entry(entity).State = EntityState.Modified;

        return await SaveAsyncChanges(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.Set<TEntity>().FindAsync(id);
        if (entity == null) return false;
        entity.DeletedAt = DateTime.UtcNow;
        return await SaveAsyncChanges();
    }

    public async Task<bool> RestoreAsync(Guid id)
    {
        var entity = await _context.Set<TEntity>().FindAsync(id);
        if (entity == null) return false;
        entity.DeletedAt = null;
        return await SaveAsyncChanges();
    }

    public virtual async Task<bool> ForceDeleteAsync(Guid id)
    {
        var entity = await _context.Set<TEntity>().FindAsync(id);

        if (entity == null)
            return false;

        _context.Set<TEntity>().Remove(entity);

        return await SaveAsyncChanges();
    }

    private async Task<bool> SaveAsyncChanges()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    private async Task<TEntity?> SaveAsyncChanges(TEntity entity)
    {
        return await _context.SaveChangesAsync() > 0 ? entity : null;
    }

    private string? TableName()
    {
        return _context.Model
            .FindEntityType(typeof(TEntity))
            ?.GetTableName();
    }
}