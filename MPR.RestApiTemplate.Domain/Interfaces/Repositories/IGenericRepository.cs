using System.Data.Common;
using System.Linq.Expressions;

namespace MPR.RestApiTemplate.Domain.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> GetAllAsync(
            params Expression<Func<TEntity, object>>[] includes);

        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes);

        Task<TEntity?> GetByIdAsync(
            params object[] keys);

        Task<TEntity?> GetByIdAsync(
            object[] keys,
            params Expression<Func<TEntity, object>>[] includes);

        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task DeleteAsync(params object[] keys);

        Task<int> ExecuteSqlRaw(string sql, params DbParameter[] dbParameters);

        Task<IEnumerable<TEntity>> FromSqlRaw(string sql, params DbParameter[] dbParameters);

        Task<int> SaveChangesAsync();
    }
}