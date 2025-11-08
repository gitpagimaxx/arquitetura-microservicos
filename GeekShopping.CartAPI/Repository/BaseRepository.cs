using GeekShopping.CartAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CartAPI.Repository;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;

    public BaseRepository(DbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> FindAll()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> FindById(long id)
    {
        var entity = await _dbSet.FindAsync(id) ?? throw new InvalidOperationException($"Entidade do tipo {typeof(T).Name} com id {id} não encontrada.");
        return entity;
    }

    public async Task<T> Create(T item)
    {
        var entity = await _dbSet.AddAsync(item);
        await _context.SaveChangesAsync();
        return entity.Entity;
    }

    public async Task<T> Update(T item)
    {
        _dbSet.Update(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<bool> Delete(long id)
    {
        var entity = await FindById(id);
        if (entity == null) return false;
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}