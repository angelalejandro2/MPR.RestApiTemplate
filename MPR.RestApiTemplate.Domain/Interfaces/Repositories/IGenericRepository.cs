﻿using System.Data.Common;
using System.Linq.Expressions;

namespace MPR.RestApiTemplate.Domain.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(params object[] keys);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task UpdateRangeAsync(IEnumerable<TEntity> entities);
        Task DeleteAsync(params object[] keys);
        Task<IEnumerable<TEntity>> FromSqlRaw(string sql, params DbParameter[] dbParameters);
        Task<int> ExecuteSqlRaw(string sql, params DbParameter[] dbParameters);
        Task<int> SaveChangesAsync();
    }
}