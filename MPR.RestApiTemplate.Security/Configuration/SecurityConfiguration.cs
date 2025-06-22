using System.Collections.Generic;

namespace MPR.RestApiTemplate.Security.Configuration;

public class SecurityConfiguration
{
    public const string SectionName = "Security";
    
    public string Provider { get; set; } = string.Empty;
    public Dictionary<string, PolicyConfiguration> Policies { get; set; } = new Dictionary<string, PolicyConfiguration>();
    public MapfreSecurityConfiguration MapfreSecurity { get; set; } = new MapfreSecurityConfiguration();
    public OktaConfiguration Okta { get; set; } = new OktaConfiguration();
    public EntraIdConfiguration EntraID { get; set; } = new EntraIdConfiguration();
}

public class MapfreSecurityConfiguration
{
    public string ApplicationId { get; set; } = string.Empty;
    public string MlicAccess { get; set; } = string.Empty;
    public string MlicExecute { get; set; } = string.Empty;
    public string TestUser { get; set; } = string.Empty;
}

public class OktaConfiguration
{
    public string Domain { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}

public class EntraIdConfiguration
{
    public string TenantId { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string Instance { get; set; } = string.Empty;
}