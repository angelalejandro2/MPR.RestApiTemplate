using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MPR.RestApiTemplate.Security.Interfaces;

namespace MPR.RestApiTemplate.Security.Authorization;

public class PermissionAuthorizationHandler(ISecurityProvider securityProvider)
    : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var username = context.User.Identity?.Name;

        if (string.IsNullOrWhiteSpace(username))
        {
            return;
        }

        var isAuthorized = await securityProvider.AuthorizeAsync(username, requirement.PolicyName);
        if (isAuthorized)
        {
            context.Succeed(requirement);
        }
    }
}
