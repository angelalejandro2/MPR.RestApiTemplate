using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using MPR.RestApiTemplate.Security.Configuration;

namespace MPR.RestApiTemplate.Security.Authorization;

public class PermissionAuthorizationHandler(IOptions<SecurityConfiguration> securityOptions)
    : AuthorizationHandler<PermissionRequirement>
{
    private readonly SecurityConfiguration _securityConfiguration = securityOptions.Value;

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        // Check if the policy exists in configuration
        if (!_securityConfiguration.Policies.TryGetValue(requirement.PolicyName, out var policyConfig))
        {
            return Task.CompletedTask;
        }

        // Get user's permission claims
        var userPermissions = context.User.Claims
            .Where(c => c.Type == "permission")
            .Select(c => c.Value)
            .ToList();

        // Check if user has all required permissions for this policy
        var hasAllPermissions = policyConfig.RequiredPermissions.All(requiredPermission =>
            userPermissions.Contains(requiredPermission));

        if (hasAllPermissions)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
