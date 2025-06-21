using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MPR.RestApiTemplate.Security.Interfaces;
using MPR.RestApiTemplate.Security.Providers;

namespace MPR.RestApiTemplate.Security.Factories;

public class SecurityProviderFactory(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
{
    public ISecurityProvider CreateProvider()
    {
        var providerName = configuration["Security:Provider"];

        return providerName switch
        {
            "MapfreSecurity" => new MapfreSecurityProvider(httpContextAccessor, configuration),
            "Okta" => throw new NotImplementedException(),//new OktaProvider(httpContextAccessor, _configuration),
            "EntraID" => throw new NotImplementedException(),// new EntraIdProvider(httpContextAccessor, _configuration),
            _ => throw new InvalidOperationException($"Security provider '{providerName}' is not supported.")
        };
    }
}
