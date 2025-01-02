using ProductsCRUD.Domain.Promotions;
using System.Linq.Expressions;

namespace ProductsCRUD.Domain._core
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAll();
        Task<IEnumerable<TEntity>> GetAllWithSearchPagination(Expression<Func<TEntity, bool>> predicate, int skip, int take);
        Task<TEntity> GetWithNoTracking(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> FindWithNotTracking(Expression<Func<TEntity, bool>> predicate);
        Task Add(TEntity entity);
        void HardRemove(TEntity entity);
        Task<bool> Any(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountWithSearch(Expression<Func<TEntity, bool>> predicate);
    }
}
