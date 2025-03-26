using System.Data.Common;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MPR.RestApiTemplate.Domain.Interfaces.Repositories;

namespace MPR.RestApiTemplate.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbSet.ToListAsync();
        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate) => await _dbSet.Where(predicate).ToListAsync();
        public virtual async Task<TEntity?> GetByIdAsync(params object[] keys) => await _dbSet.FindAsync(keys);
        public virtual async Task<TEntity> AddAsync(TEntity entity) => (await _dbSet.AddAsync(entity)).Entity;
        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities) => await _dbSet.AddRangeAsync(entities);
        public virtual async Task<TEntity> UpdateAsync(TEntity entity) => await Task.Run(() => _dbSet.Update(entity).Entity);
        public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities) => await Task.Run(() => _dbSet.UpdateRange(entities));
        public virtual async Task DeleteAsync(params object[] keys)
        {
            var entity = await _dbSet.FindAsync(keys);
            if (entity != null) _dbSet.Remove(entity);
        }

        public virtual async Task<int> ExecuteSqlRaw(string sql, params DbParameter[] dbParameters) => await _dbContext.Database.ExecuteSqlRawAsync(sql, dbParameters);
        public virtual async Task<IEnumerable<TEntity>> FromSqlRaw(string sql, params DbParameter[] dbParameters) => await _dbSet.FromSqlRaw(sql, dbParameters).ToListAsync();
        public virtual async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();
    }
}
