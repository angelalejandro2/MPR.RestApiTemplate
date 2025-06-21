using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using MapfreUserSecurityLibrary.Implementation;
using MPR.RestApiTemplate.Security.Interfaces;

public class MapfreSecurityProvider(IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : ISecurityProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IConfiguration _configuration = configuration;

    public async Task<bool> AuthenticateAsync(HttpContext context)
    {
        return context.User?.Identity?.IsAuthenticated == true;
    }

    public async Task<ClaimsPrincipal> CreateClaimsPrincipalAsync(string username)
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

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.RoleID));
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
        if (context?.User?.Identity?.IsAuthenticated != true)
            return null;

        var fullUser = context.User.Identity.Name;
        var userId = fullUser?.Split('\\').Last();

        // Soporte para testing desde config
        var testUser = _configuration["Security:MapfreSecurity:TestUser"];
        if (!string.IsNullOrEmpty(testUser))
            userId = testUser;

        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "IP Address Not Available";

        var user = new clsUsers(userId, clsUsers.AccountTypes.ActiveDirectory, ip);

        if (user.NumUserID <= 0)
            return null;

        var appId = Convert.ToInt32(_configuration["Security:MapfreSecurity:ApplicationId"]);

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
}
