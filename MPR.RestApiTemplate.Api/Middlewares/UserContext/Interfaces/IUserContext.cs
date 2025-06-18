using MapfreUserSecurityLibrary.Entities;

namespace MPR.RestApiTemplate.Api.Middlewares.UserContext.Interfaces
{
    public interface IUserContext
    {
        string UserId { get; }
        long NumUserId { get; }
        string TraceId { get; }
        string IpAddress { get; }

        int AppId { get; }
        List<string> AppValidRoles { get; }
        List<UserRoles> UserAppRoles { get; }

        HttpRequest HttpRequest { get; }
        
    }
}
