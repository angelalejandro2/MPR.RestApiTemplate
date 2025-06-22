using System.Collections.Generic;

namespace MPR.RestApiTemplate.Api.Security.Configuration;

public class SecurityConfiguration
{
    public const string SectionName = "Security";
    
    public JwtConfiguration Jwt { get; set; } = new JwtConfiguration();
    public Dictionary<string, PolicyConfiguration> Policies { get; set; } = new Dictionary<string, PolicyConfiguration>();
}

public class JwtConfiguration
{
    public string Issuer { get; set; } = string.Empty;
    public List<string> Audiences { get; set; } = new List<string>();
    public string SecretKey { get; set; } = string.Empty;
}