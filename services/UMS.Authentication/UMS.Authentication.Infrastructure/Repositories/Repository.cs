using UMS.Authentication.Infrastructure.Interfaces;
using UMS.Authentication.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace UMS.Authentication.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T?>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<int> CreateAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _context.Set<T>().Add(entity);

        _context.Entry(entity).State = EntityState.Added;

        return await _context.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _context.Entry(entity).State = EntityState.Modified;

        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(int id)
    {
        var entity = await _context.Set<T>().FindAsync(id);

        if (entity == null)
            return -1;

        _context.Set<T>().Remove(entity);

        return await _context.SaveChangesAsync();
    }
}