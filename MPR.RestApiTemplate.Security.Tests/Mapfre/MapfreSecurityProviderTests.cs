using MapfreUserSecurityLibrary.Implementation;
using Microsoft.AspNetCore.Http;
using Moq;
using MPR.RestApiTemplate.Security.Providers;
using MPR.RestApiTemplate.Security.Tests.Common;
using System.Configuration;
using System.Security.Claims;
using Xunit;

namespace MPR.RestApiTemplate.Security.Tests.Mapfre;


public class MapfreSecurityProviderTests : TestBase
{
    [Fact]
    public void Should_Read_ConnectionString_From_AppConfig()
    {
        var config = GetTestConfiguration();

        ConfigurationManager.AppSettings["ExtranetPortal.ConnectionString"] =
            config["ExtranetPortal.ConnectionString"];
        var connStr = clsUtilities.GetConfigValue("ExtranetPortal.ConnectionString");
        Assert.False(string.IsNullOrWhiteSpace(connStr));
    }

    [Fact]
    public async Task CreateClaimsPrincipal_ReturnsPrincipal_WhenUserIsValid()
    {
        var config = GetTestConfiguration();
        var accessor = MockHttpContextWithBasicAuth("fpereira", config["Security:MapfreSecurity:ApplicationId"]);

        var provider = new MapfreSecurityProvider(accessor, config);

        ClaimsPrincipal result;
        try
        {
            result = await provider.CreateClaimsPrincipalAsync("fpereira");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }

        Assert.NotNull(result);
        Assert.Contains(result.Claims, c => c.Type == ClaimTypes.Name);
    }

    [Fact]
    public async Task AuthenticateAsync_ReturnsTrue_WhenUserIsAuthenticated()
    {
        var config = GetTestConfiguration();
        var context = new DefaultHttpContext();
        var credentials = $"fpereira:{config["Security:MapfreSecurity:ApplicationId"]}";
        var encodedCredentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(credentials));
        context.Request.Headers["Authorization"] = $"Basic {encodedCredentials}";

        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(x => x.HttpContext).Returns(context);

        var provider = new MapfreSecurityProvider(accessor.Object, config);

        var result = await provider.AuthenticateAsync(context);

        Assert.True(result);
    }

    [Fact]
    public async Task AuthenticateAsync_ReturnsFalse_WhenUserIsNotAuthenticated()
    {
        var context = new DefaultHttpContext(); // User is null

        var config = GetTestConfiguration();
        var provider = new MapfreSecurityProvider(Mock.Of<IHttpContextAccessor>(), config);

        var result = await provider.AuthenticateAsync(context);

        Assert.False(result);
    }

    [Fact]
    public async Task GetUserPermissionsAsync_ReturnsEmpty_WhenUserIsInvalid()
    {
        var config = GetTestConfiguration();
        var accessor = MockHttpContextWithBasicAuth("fpereira", config["Security:MapfreSecurity:ApplicationId"]);

        var provider = new MapfreSecurityProvider(accessor, config);

        var result = await provider.GetUserPermissionsAsync("fpereira");

        Assert.NotEmpty(result);
    }


    [Fact]
    public async Task GetUserPermissionsAsync_ReturnsArray_WhenMatches()
    {
        var config = GetTestConfiguration();
        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(x => x.HttpContext).Returns(new DefaultHttpContext());

        var provider = new MapfreSecurityProvider(accessor.Object, config);

        var result = await provider.GetUserPermissionsAsync("invalidUser");

        Assert.Empty(result);
    }

    [Fact]
    public async Task AuthorizeAsync_ReturnsTrue_WhenPermissionMatches()
    {
        var config = GetTestConfiguration();
        var accessor = MockHttpContextWithBasicAuth("fpereira", config["Security:MapfreSecurity:ApplicationId"]);

        var provider = new MapfreSecurityProvider(accessor, config);

        var result = await provider.AuthorizeAsync("fpereira", config["Security:MapfreSecurity:MlicAccess"]);

        Assert.True(result);
    }


}