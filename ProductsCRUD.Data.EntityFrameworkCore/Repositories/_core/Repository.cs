using Microsoft.EntityFrameworkCore;
using ProductsCRUD.Domain._core;
using System.Linq.Expressions;

namespace ProductsCRUD.Data.EntityFrameworkCore.Repositories._core
{
    public class Repository<TEntity>(DbContext context) : IRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _entities = context.Set<TEntity>();

        public async Task Add(TEntity entity)
        {
            await _entities.AddAsync(entity);
        }

        public async Task<IEnumerable<TEntity>> FindWithNotTracking(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entities.Where(predicate).AsNoTracking().ToListAsync();
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entities.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _entities.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllWithSearchPagination(Expression<Func<TEntity, bool>> predicate, int skip, int take)
        {
            return await _entities.Where(predicate).Skip(skip).Take(take).AsNoTracking().ToListAsync();
        }

        public async Task<TEntity> GetWithNoTracking(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entities.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public void HardRemove(TEntity entity)
        {
            _entities.Remove(entity);
        }

        public async Task<bool> Any(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entities.AnyAsync(predicate);
        }

        public async Task<int> CountWithSearch(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entities.Where(predicate).AsNoTracking().CountAsync();
        }


    }
}
