using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using MPR.RestApiTemplate.Domain.Interfaces.Repositories;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace MPR.RestApiTemplate.Infrastructure.Helpers
{
    public static class DatabaseUtils
    {
        public static async Task<IEnumerable<TResult>> ExecuteStoredProcedureAsync<TResult>(
            DbContext dbContext,
            string storedProcedureName,
            Func<DbDataReader, TResult> map,
            IEnumerable<DbParameterDefinition>? parameters = null,
            string? cursorParameterName = null)
        {
            var conn = dbContext.Database.GetDbConnection();
            await dbContext.Database.OpenConnectionAsync();

            using var command = conn.CreateCommand();
            command.CommandText = storedProcedureName;
            command.CommandType = CommandType.StoredProcedure;

            bool isOracle = IsOracle(dbContext);

            if (parameters != null)
            {
                foreach (var paramDef in parameters)
                {
                    command.Parameters.Add(CreateProviderParameter(command, paramDef, isOracle));
                }
            }

            var results = new List<TResult>();

            if (isOracle)
            {
                if (string.IsNullOrEmpty(cursorParameterName))
                    throw new ArgumentException("Oracle stored procedure requires a REF CURSOR output parameter name.");

                var cursor = new OracleParameter
                {
                    ParameterName = cursorParameterName,
                    OracleDbType = OracleDbType.RefCursor,
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(cursor);
            }

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(map(reader));
            }

            return results;
        }

        public static async Task<IEnumerable<TResult>> ExecuteSqlQueryAsync<TResult>(
           DbContext dbContext,
           string sqlQuery,
           Func<DbDataReader, TResult> map,
           IEnumerable<DbParameterDefinition>? parameters = null)
        {
            var conn = dbContext.Database.GetDbConnection();
            await dbContext.Database.OpenConnectionAsync();

            using var command = conn.CreateCommand();
            command.CommandText = sqlQuery;
            command.CommandType = CommandType.Text;

            bool isOracle = IsOracle(dbContext);

            if (parameters != null)
            {
                foreach (var paramDef in parameters)
                {
                    command.Parameters.Add(CreateProviderParameter(command, paramDef, isOracle));
                }
            }

            var results = new List<TResult>();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(map(reader));
            }

            return results;
        }

        private static DbParameter CreateProviderParameter(DbCommand command, DbParameterDefinition def, bool isOracle)
        {
            var param = command.CreateParameter();
            param.ParameterName = def.Name;
            param.Value = def.Value ?? DBNull.Value;
            param.Direction = def.Direction;

            if (def.DbType.HasValue)
                param.DbType = def.DbType.Value;

            return param;
        }

        private static bool IsOracle(DbContext dbContext)
        {
            return dbContext.Database.ProviderName?.ToLower().Contains("oracle", StringComparison.CurrentCultureIgnoreCase) ?? false;
        }
    }
}
