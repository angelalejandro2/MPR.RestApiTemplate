using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Configuration;
using System.IO;

namespace MPR.RestApiTemplate.Security.Tests.Common
{
    public abstract class TestBase
    {
        protected Microsoft.Extensions.Configuration.IConfiguration GetTestConfiguration()
        {
            var config = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)!.FullName)
                .AddJsonFile("appsettings_test.json", optional: false)
                .Build();


            System.Configuration.ConfigurationManager.AppSettings["ExtranetPortal.ConnectionString"] = config["ExtranetPortal.ConnectionString"];
            System.Configuration.ConfigurationManager.AppSettings["ApplicationId"] = config["Security:MapfreSecurity:ApplicationId"];
            System.Configuration.ConfigurationManager.AppSettings["MlicAccess"] = config["Security:MapfreSecurity:MlicAccess"];
            System.Configuration.ConfigurationManager.AppSettings["MlicExecute"] = config["Security:MapfreSecurity:MlicExecute"];
            System.Configuration.ConfigurationManager.AppSettings["ADUserTest"] = config["Security:MapfreSecurity:TestUser"];
            System.Configuration.ConfigurationManager.AppSettings["ADUserTestIsOn"] = "Y";
            System.Configuration.ConfigurationManager.AppSettings["EncrypDecrypKey"] = "prueba";
            System.Configuration.ConfigurationManager.AppSettings["TipUsr"] = config["Security:MapfreSecurity:TipUsr"];

            return config;
        }

        protected IHttpContextAccessor MockHttpContextWithUser(string windowsUsername)
        {
            var context = new DefaultHttpContext();
            context.User = new System.Security.Claims.ClaimsPrincipal(
                new System.Security.Claims.ClaimsIdentity(
                    new[] {
                        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, windowsUsername)
                    },
                    "Windows"
                )
            );

            var mock = new Mock<IHttpContextAccessor>();
            mock.Setup(m => m.HttpContext).Returns(context);

            return mock.Object;
        }

        protected IHttpContextAccessor MockHttpContextWithBasicAuth(string username, string applicationId)
        {
            var context = new DefaultHttpContext();
            var credentials = $"{username}:{applicationId}";
            var encodedCredentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(credentials));
            
            context.Request.Headers["Authorization"] = $"Basic {encodedCredentials}";

            var mock = new Mock<IHttpContextAccessor>();
            mock.Setup(m => m.HttpContext).Returns(context);

            return mock.Object;
        }
    }
}
