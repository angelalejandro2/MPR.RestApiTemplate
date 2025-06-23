using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MPR.RestApiTemplate.Application.Helpers
{
    public static class PredicateExpressionBuilder
    {
        // Example filter: "Department.Name='HR';Manager.Age=30;IsActive=true"
        // Supports: string (with single quotes), int, bool, DateTime (with single quotes), double, etc.
        public static Expression<Func<T, bool>> BuildPredicate<T>(string? filters)
        {
            var filterPairs = (filters ?? string.Empty)
                .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var param = Expression.Parameter(typeof(T), "e");
            Expression? body = null;

            foreach (var filter in filterPairs)
            {
                var parts = filter.Split('=', 2, StringSplitOptions.TrimEntries);
                if (parts.Length != 2) continue;

                var path = parts[0];
                var value = parts[1];

                Expression propertyAccess = param;
                Type currentType = typeof(T);

                foreach (var prop in path.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    var property = currentType.GetProperty(prop, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (property == null)
                    {
                        propertyAccess = null!;
                        break;
                    }
                    propertyAccess = Expression.Property(propertyAccess, property);
                    currentType = property.PropertyType;
                }

                if (propertyAccess != null)
                {
                    object? typedValue = ParseValue(value, currentType);
                    Expression equality;
                    if (currentType == typeof(string))
                    {
                        // Use case-insensitive comparison for strings
                        var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                        var left = Expression.Call(propertyAccess, toLowerMethod!);
                        var right = Expression.Constant(((string)typedValue!).ToLowerInvariant(), typeof(string));
                        equality = Expression.Equal(left, right);
                    }
                    else
                    {
                        var constant = Expression.Constant(typedValue, currentType);
                        equality = Expression.Equal(propertyAccess, constant);
                    }

                    body = body == null ? equality : Expression.AndAlso(body, equality);
                }
            }

            if (body == null)
            {
                // No valid filters, return a predicate that always returns true
                return e => true;
            }

            return Expression.Lambda<Func<T, bool>>(body, param);
        }

        private static object? ParseValue(string value, Type targetType)
        {
            if (targetType == typeof(string))
            {
                // Remove single quotes if present
                if (value.StartsWith("'", StringComparison.Ordinal) && value.EndsWith("'", StringComparison.Ordinal))
                {
                    value = value.Substring(1, value.Length - 2);
                }
                return value;
            }
            if (targetType == typeof(DateTime) || targetType == typeof(DateTime?))
            {
                if (value.StartsWith("'", StringComparison.Ordinal) && value.EndsWith("'", StringComparison.Ordinal))
                {
                    value = value.Substring(1, value.Length - 2);
                }
                return DateTime.Parse(value, CultureInfo.InvariantCulture);
            }
            if (targetType.IsEnum)
            {
                return Enum.Parse(targetType, value, ignoreCase: true);
            }
            if (targetType == typeof(bool) || targetType == typeof(bool?))
            {
                return bool.Parse(value);
            }
            if (targetType == typeof(int) || targetType == typeof(int?))
            {
                return int.Parse(value, CultureInfo.InvariantCulture);
            }
            if (targetType == typeof(long) || targetType == typeof(long?))
            {
                return long.Parse(value, CultureInfo.InvariantCulture);
            }
            if (targetType == typeof(double) || targetType == typeof(double?))
            {
                return double.Parse(value, CultureInfo.InvariantCulture);
            }
            if (targetType == typeof(decimal) || targetType == typeof(decimal?))
            {
                return decimal.Parse(value, CultureInfo.InvariantCulture);
            }
            if (Nullable.GetUnderlyingType(targetType) != null)
            {
                // Nullable type
                var underlyingType = Nullable.GetUnderlyingType(targetType)!;
                return ParseValue(value, underlyingType);
            }
            // Fallback
            return Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
        }
    }
}