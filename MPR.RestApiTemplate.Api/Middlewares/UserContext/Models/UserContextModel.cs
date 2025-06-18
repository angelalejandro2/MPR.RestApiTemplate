using MapfreUserSecurityLibrary.Entities;
using MPR.RestApiTemplate.Api.Middlewares.UserContext.Interfaces;

namespace MPR.RestApiTemplate.Api.Middlewares.UserContext.Models
{
    internal class UserContextModel : IUserContextInternal
    {
        public string UserId { get; set; } = string.Empty;
        public long NumUserId { get; set; }
        public string TraceId { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public HttpRequest HttpRequest { get; set; }
        public int AppId { get; set; }
        public List<string> AppValidRoles { get; set; }
        public List<UserRoles> UserAppRoles { get; set; }
    }
}
