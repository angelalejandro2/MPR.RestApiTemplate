using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using MapfreUserSecurityLibrary.Implementation;
using MPR.RestApiTemplate.Security.Interfaces;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPR.RestApiTemplate.Security.Providers;

public class MapfreSecurityProvider(IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : ISecurityProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IConfiguration _configuration = configuration;

    public async Task<bool> AuthenticateAsync(HttpContext context)
    {
        var credentials = ParseBasicAuthHeader(context);
        return credentials != null;
    }

    public async Task<ClaimsPrincipal?> CreateClaimsPrincipalAsync(string username)
    {
        var context = _httpContextAccessor.HttpContext;
        var user = GetValidatedUser(context);

        if (user == null)
            return null;

        var appId = Convert.ToInt32(_configuration["Security:MapfreSecurity:ApplicationId"]);
        var roles = clsApplication.GetUserApplicationRoles(appId, user.NumUserID);

        var claims = new List<Claim>
        {
            new (ClaimTypes.Name, user.UserID),
            new (ClaimTypes.Email, user.Email),
            new (ClaimTypes.Surname, user.LastName),
            new (ClaimTypes.NameIdentifier, user.UserID),
        };

        // Add role claims for backward compatibility
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.RoleID));
        }

        // Add permission claims - TODO: Replace with actual permission retrieval method from MapfreUserSecurityLibrary
        var userPermissions = await GetUserPermissionsFromLibraryAsync((int)user.NumUserID, appId);
        foreach (var permission in userPermissions)
        {
            claims.Add(new Claim("permission", permission));
        }

        var identity = new ClaimsIdentity(claims, "MapfreSecurity");        
        return new ClaimsPrincipal(identity);
    }

    public async Task<IEnumerable<string>> GetUserPermissionsAsync(string username)
    {
        var context = _httpContextAccessor.HttpContext;
        var user = GetValidatedUser(context);

        if (user == null)
            return [];

        var appId = Convert.ToInt32(_configuration["Security:MapfreSecurity:ApplicationId"]);
        var roles = clsApplication.GetUserApplicationRoles(appId, user.NumUserID);

        return roles.Select(r => r.RoleID).Distinct();
    }

    public async Task<bool> AuthorizeAsync(string username, string policy)
    {
        var permissions = await GetUserPermissionsAsync(username);
        return permissions.Contains(policy, StringComparer.OrdinalIgnoreCase);
    }

    private clsUsers? GetValidatedUser(HttpContext context)
    {
        var credentials = ParseBasicAuthHeader(context);
        if (credentials == null)
            return null;

        var userId = credentials.Value.Username;
        var providedAppId = credentials.Value.ApplicationId;

        // Validate applicationId against configuration
        var configuredAppId = _configuration["Security:MapfreSecurity:ApplicationId"];
        if (providedAppId != configuredAppId)
            return null;

        // Soporte para testing desde config
        var testUser = _configuration["Security:MapfreSecurity:TestUser"];
        if (!string.IsNullOrEmpty(testUser))
            userId = testUser;

        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "IP Address Not Available";

        var user = new clsUsers(userId, clsUsers.AccountTypes.ActiveDirectory, ip);

        if (user.NumUserID <= 0)
            return null;

        var appId = Convert.ToInt32(configuredAppId);

        var validRoles = new[]
        {
            _configuration["Security:MapfreSecurity:MlicAccess"],
            _configuration["Security:MapfreSecurity:MlicExecute"]
        }.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

        var profiles = clsApplication.GetUserApplicationRoles(appId, user.NumUserID);

        if (!profiles.Any(p => validRoles.Contains(p.RoleID, StringComparer.OrdinalIgnoreCase)))
            return null;

        return user;
    }

    private (string Username, string ApplicationId)? ParseBasicAuthHeader(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey("Authorization"))
            return null;

        var authHeader = context.Request.Headers["Authorization"].ToString();
        if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            return null;

        try
        {
            var encodedCredentials = authHeader.Substring(6); // Remove "Basic " prefix
            var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
            
            var colonIndex = decodedCredentials.IndexOf(':');
            if (colonIndex == -1)
                return null;

            var username = decodedCredentials.Substring(0, colonIndex);
            var applicationId = decodedCredentials.Substring(colonIndex + 1);

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(applicationId))
                return null;

            return (username, applicationId);
        }
        catch
        {
            return null;
        }
    }

    private async Task<IEnumerable<string>> GetUserPermissionsFromLibraryAsync(int userId, int appId)
    {
        // TODO: Replace this placeholder with actual call to MapfreUserSecurityLibrary
        // This method should retrieve user permissions from the security library
        // Example call might be something like:
        // var permissions = clsApplication.GetUserApplicationPermissions(appId, userId);
        // return permissions.Select(p => p.PermissionName);

        // For now, return permissions based on roles as a fallback
        var roles = clsApplication.GetUserApplicationRoles(appId, userId);
        var permissions = new List<string>();

        // Map roles to permissions (this is temporary until actual permission retrieval is implemented)
        foreach (var role in roles)
        {
            if (role.RoleID.Equals(_configuration["Security:MapfreSecurity:MlicAccess"], StringComparison.OrdinalIgnoreCase))
            {
                permissions.AddRange(new[] { "AppRead", "DataAccess" });
            }
            if (role.RoleID.Equals(_configuration["Security:MapfreSecurity:MlicExecute"], StringComparison.OrdinalIgnoreCase))
            {
                permissions.AddRange(new[] { "AppRead", "AppWrite", "AppDelete", "DataAccess", "DataCreate", "DataModify", "DataRemove", "AppAdmin" });
            }
        }

        return permissions.Distinct();
    }

    public string? GetUsernameFromContext(HttpContext context)
    {
        var credentials = ParseBasicAuthHeader(context);
        if (credentials == null)
            return null;

        var username = credentials.Value.Username;
        
        // Support for testing from config - override username if TestUser is configured
        var testUser = _configuration["Security:MapfreSecurity:TestUser"];
        if (!string.IsNullOrEmpty(testUser))
            username = testUser;

        return username;
    }
}
