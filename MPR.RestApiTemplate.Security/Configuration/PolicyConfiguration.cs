using System.Collections.Generic;

namespace MPR.RestApiTemplate.Security.Configuration;

public class PolicyConfiguration
{
    public List<string> RequiredPermissions { get; set; } = new List<string>();
}