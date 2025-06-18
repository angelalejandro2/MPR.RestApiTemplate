using System.Data.Common;

namespace MPR.RestApiTemplate.Domain.Interfaces.Repositories
{
    public interface ISqlExecutor
    {
        Task<IEnumerable<TResult>> ExecuteStoredProcedureAsync<TResult>(
            string storedProcedureName,
            Func<DbDataReader, TResult> map,
            IEnumerable<DbParameterDefinition>? parameters = null,
            string? cursorParameterName = null);

        Task<IEnumerable<TResult>> ExecuteSqlQueryAsync<TResult>(
            string sqlQuery,
            Func<DbDataReader, TResult> map,
            IEnumerable<DbParameterDefinition>? parameters = null);
    }
}
