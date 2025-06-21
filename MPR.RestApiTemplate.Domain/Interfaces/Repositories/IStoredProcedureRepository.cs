using System.Data.Common;

namespace MPR.RestApiTemplate.Domain.Interfaces.Repositories
{
    public interface IStoredProcedureRepository
    {
        Task<IEnumerable<TResult>> ExecuteStoredProcedureAsync<TResult>(
            string storedProcedureName,
            Func<DbDataReader, TResult> map,
            IEnumerable<DbParameterDefinition>? parameters = null,
            string? cursorParameterName = null);
    }
}
