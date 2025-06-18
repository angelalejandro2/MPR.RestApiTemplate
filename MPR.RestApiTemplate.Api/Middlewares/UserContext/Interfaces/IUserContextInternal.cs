using MapfreUserSecurityLibrary.Entities;

namespace MPR.RestApiTemplate.Api.Middlewares.UserContext.Interfaces
{
    internal interface IUserContextInternal: IUserContext
    {
        new string UserId { get; set; }
        new long NumUserId { get; set; }
        new string TraceId { get; set; }
        new string IpAddress { get; set; }

        new int AppId { get; set; }
        new List<string> AppValidRoles { get; set; }
        new List<UserRoles> UserAppRoles { get; set; }

        new HttpRequest HttpRequest { get; set; }
    }
}
