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

        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
            => await _dbSet.Where(predicate).ToListAsync();

        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.Where(predicate).ToListAsync();
        }

        public virtual async Task<TEntity?> GetByIdAsync(params object[] keys)
            => await _dbSet.FindAsync(keys);

        public virtual async Task<TEntity?> GetByIdAsync(
            object[] keys,
            params Expression<Func<TEntity, object>>[] includes)
        {
            var entityType = _dbContext.Model.FindEntityType(typeof(TEntity));
            var key = entityType?.FindPrimaryKey();

            if (key == null)
                throw new InvalidOperationException($"Entity {typeof(TEntity).Name} does not have a primary key defined.");

            if (key.Properties.Count != keys.Length)
                throw new ArgumentException("Incorrect number of key values.");

            var parameter = Expression.Parameter(typeof(TEntity), "e");
            Expression? predicate = null;

            for (int i = 0; i < key.Properties.Count; i++)
            {
                var property = Expression.Property(parameter, key.Properties[i].Name);
                var constant = Expression.Constant(Convert.ChangeType(keys[i], property.Type));
                var equality = Expression.Equal(property, constant);

                predicate = predicate == null ? equality : Expression.AndAlso(predicate, equality);
            }

            var lambda = Expression.Lambda<Func<TEntity, bool>>(predicate!, parameter);

            IQueryable<TEntity> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(lambda);
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
            => (await _dbSet.AddAsync(entity)).Entity;

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
            => await Task.Run(() => _dbSet.Update(entity).Entity);

        public virtual async Task DeleteAsync(params object[] keys)
        {
            var entity = await _dbSet.FindAsync(keys);
            if (entity != null) _dbSet.Remove(entity);
        }

        public virtual async Task<int> ExecuteSqlRaw(string sql, params DbParameter[] dbParameters)
            => await _dbContext.Database.ExecuteSqlRawAsync(sql, dbParameters);

        public virtual async Task<IEnumerable<TEntity>> FromSqlRaw(string sql, params DbParameter[] dbParameters)
            => await _dbSet.FromSqlRaw(sql, dbParameters).ToListAsync();

        public virtual async Task<int> SaveChangesAsync()
            => await _dbContext.SaveChangesAsync();
    }
}