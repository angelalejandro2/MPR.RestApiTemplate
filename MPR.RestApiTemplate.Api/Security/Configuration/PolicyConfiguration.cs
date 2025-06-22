using System.Collections.Generic;

namespace MPR.RestApiTemplate.Api.Security.Configuration;

public class PolicyConfiguration
{
    public List<string> RequiredPermissions { get; set; } = new List<string>();
}