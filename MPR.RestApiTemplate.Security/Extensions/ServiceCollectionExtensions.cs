using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MPR.RestApiTemplate.Security.Authorization;
using MPR.RestApiTemplate.Security.Configuration;
using MPR.RestApiTemplate.Security.Factories;
using MPR.RestApiTemplate.Security.Interfaces;

namespace MPR.RestApiTemplate.Security.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSecurityServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add HTTP context accessor manually
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        // Bind security configuration
        services.Configure<SecurityConfiguration>(configuration.GetSection(SecurityConfiguration.SectionName));

        // Add security provider factory and services
        services.AddSingleton<SecurityProviderFactory>();
        services.AddScoped<ISecurityProvider>(sp => sp.GetRequiredService<SecurityProviderFactory>().CreateProvider());

        // Add authorization handler
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

        // Note: Authorization policies will be configured in the host application
        // since this library cannot add the full authorization services

        return services;
    }
}