using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MPR.RestApiTemplate.Infrastructure.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void UseOracleUpperCaseNamingConvention(this ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.GetTableName()?.ToUpperInvariant());

                foreach (var property in entity.GetProperties())
                    property.SetColumnName(property.GetColumnName()?.ToUpperInvariant());

                foreach (var key in entity.GetKeys())
                    key.SetName(key.GetName()?.ToUpperInvariant());

                foreach (var fk in entity.GetForeignKeys())
                    fk.SetConstraintName(fk.GetConstraintName()?.ToUpperInvariant());

                foreach (var index in entity.GetIndexes())
                    index.SetDatabaseName(index.GetDatabaseName()?.ToUpperInvariant());
            }
        }
    }
}
