using Microsoft.AspNetCore.Builder;
using MPR.RestApiTemplate.Security.Middleware;

namespace MPR.RestApiTemplate.Security.Extensions;

public static class SecurityMiddlewareExtensions
{
    public static IApplicationBuilder UseSecurityMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SecurityMiddleware>();
    }
}