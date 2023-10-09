using DataAccess.Data;
using DataAccess.Interfaces;
using Humanizer;
using MeatAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MeatAPI.Features
{
    public class EntityAccessServiceBase<TContext, TEntity> where TContext : DbContext where TEntity : class, IBaseEntity
    {
        protected readonly TContext _dbContext;
        protected DbSet<TEntity> _dbSet;

        public EntityAccessServiceBase(TContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public virtual Task<List<TEntity>> GetAll()
        {
            return _dbSet.AsNoTracking().ToListAsync();
        }

        public virtual Task<TEntity> Get(Guid id)
        {
            return _dbSet.AsNoTracking().FirstAsync(e => e.Id == id);
        }

        public virtual async Task<Guid> Create(TEntity e)
        {
            await _dbSet.AddAsync(e);
            await _dbContext.SaveChangesAsync();
            return e.Id;
        }

        public virtual async Task Update(TEntity e)
        {
            _dbSet.Update(e);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task Delete(Guid id)
        {
            var e = await _dbSet.FirstAsync(e => e.Id == id);
            _dbSet.Remove(e);
            await _dbContext.SaveChangesAsync();
        }
    }
}
