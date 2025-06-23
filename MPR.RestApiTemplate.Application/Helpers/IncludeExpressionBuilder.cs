using System.Linq.Expressions;
using System.Reflection;

namespace MPR.RestApiTemplate.Application.Helpers
{
    public static class IncludeExpressionBuilder
    {
        public static Expression<Func<T, object>>[] BuildIncludeExpressions<T>(string? includes)
        {
            var includePaths = (includes ?? string.Empty)
                .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var includeExpressions = new List<Expression<Func<T, object>>>();

            foreach (var path in includePaths)
            {
                var param = Expression.Parameter(typeof(T), "e");
                Expression body = param;
                Type currentType = typeof(T);

                foreach (var prop in path.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    var property = currentType.GetProperty(prop, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (property == null)
                    {
                        body = null;
                        break;
                    }
                    body = Expression.Property(body, property);
                    currentType = property.PropertyType;
                }

                if (body != null)
                {
                    // Only convert to object if the property is a value type
                    Expression finalBody = currentType.IsValueType ? Expression.Convert(body, typeof(object)) : body;
                    var lambda = Expression.Lambda<Func<T, object>>(finalBody, param);
                    includeExpressions.Add(lambda);
                }
            }

            return includeExpressions.ToArray();
        }
    }
}
