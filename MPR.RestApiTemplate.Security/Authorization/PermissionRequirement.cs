using Microsoft.AspNetCore.Authorization;

namespace MPR.RestApiTemplate.Security.Authorization;

public class PermissionRequirement(string policyName) : IAuthorizationRequirement
{
    public string PolicyName { get; } = policyName;
}
