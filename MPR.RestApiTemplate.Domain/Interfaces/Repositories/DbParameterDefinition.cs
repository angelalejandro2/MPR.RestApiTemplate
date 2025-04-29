using System.Data;

namespace MPR.RestApiTemplate.Domain.Interfaces.Repositories
{
    public class DbParameterDefinition
    {
        public DbParameterDefinition()
        {
        }

        public DbParameterDefinition(string Name, object? Value, DbType? DbType = null, ParameterDirection Direction = ParameterDirection.Input)
        {
            this.Name = Name;
            this.Value = Value;
            this.DbType = DbType;
            this.Direction = Direction;
        }

        public string Name { get; set; } = default!;
        public object? Value { get; set; }
        public DbType? DbType { get; set; }
        public ParameterDirection Direction { get; set; } = ParameterDirection.Input;
    }
}
