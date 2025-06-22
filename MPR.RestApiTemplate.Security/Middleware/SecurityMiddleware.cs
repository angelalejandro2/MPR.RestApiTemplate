using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MPR.RestApiTemplate.Security.Interfaces;

namespace MPR.RestApiTemplate.Security.Middleware;

public class SecurityMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var provider = context.RequestServices.GetRequiredService<ISecurityProvider>();

        // Authentication
        var authenticated = await provider.AuthenticateAsync(context);
        if (!authenticated)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }

        // Claims principal creation
        var username = provider.GetUsernameFromContext(context);
        if (!string.IsNullOrWhiteSpace(username))
        {
            var principal = await provider.CreateClaimsPrincipalAsync(username);
            if (principal != null)
            {
                context.User = principal;
            }
        }

        await _next(context);
    }
}