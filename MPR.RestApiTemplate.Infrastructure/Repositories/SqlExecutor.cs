using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using MPR.RestApiTemplate.Domain.Interfaces.Repositories;
using MPR.RestApiTemplate.Infrastructure.Helpers;

namespace MPR.RestApiTemplate.Infrastructure.Repositories
{
    public class SqlExecutor(DbContext dbContext) : ISqlExecutor
    {
        private readonly DbContext _dbContext = dbContext;

        public async Task<IEnumerable<TResult>> ExecuteSqlQueryAsync<TResult>(
            string sqlQuery, 
            Func<DbDataReader, TResult> map, 
            IEnumerable<DbParameterDefinition>? parameters = null)
        {
            return await DatabaseUtils.ExecuteSqlQueryAsync(
                _dbContext,
                sqlQuery,
                map,
                parameters
            );
        }

        public async Task<IEnumerable<TResult>> ExecuteStoredProcedureAsync<TResult>(
            string storedProcedureName,
            Func<DbDataReader, TResult> map,
            IEnumerable<DbParameterDefinition>? parameters = null,
            string? cursorParameterName = null)
        {
            return await DatabaseUtils.ExecuteStoredProcedureAsync(
                _dbContext,
                storedProcedureName,
                map,
                parameters,
                cursorParameterName
            );
        }
    }
}
