using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MPR.RestApiTemplate.Security.Interfaces
{
    public interface ISecurityProvider
    {
        Task<bool> AuthenticateAsync(HttpContext context);
        Task<ClaimsPrincipal> CreateClaimsPrincipalAsync(string username);
        Task<bool> AuthorizeAsync(string username, string policy);
        Task<IEnumerable<string>> GetUserPermissionsAsync(string username);
    }
}
