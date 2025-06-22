using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MPR.RestApiTemplate.Api.Security.Authorization;
using MPR.RestApiTemplate.Api.Security.Configuration;
using System.Text;

namespace MPR.RestApiTemplate.Api.Security.Extensions;

public static class SecurityExtensions
{
    public static IServiceCollection AddJwtSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        // Bind security configuration
        services.Configure<SecurityConfiguration>(configuration.GetSection(SecurityConfiguration.SectionName));
        
        var securityConfig = configuration.GetSection(SecurityConfiguration.SectionName).Get<SecurityConfiguration>()
            ?? throw new InvalidOperationException("Security configuration is missing");

        // Add JWT Authentication
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = securityConfig.Jwt.Issuer,
                    ValidAudiences = securityConfig.Jwt.Audiences,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(securityConfig.Jwt.SecretKey))
                };
            });

        // Add authorization handler for policy-based permissions
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

        return services;
    }
}