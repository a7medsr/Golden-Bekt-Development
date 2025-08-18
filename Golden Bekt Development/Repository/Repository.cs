using Golden_Bekt_Development.Models.Context;
using Microsoft.EntityFrameworkCore;
using Golden_Bekt_Development.Models;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly GoldenDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(GoldenDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            if (entity is Common commonEntity)
            {
                commonEntity.IsActive = 0;
                commonEntity.IsDelete = 1;
                commonEntity.DeletedAt = DateTime.UtcNow;
            }

            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
