using System.Net;
using MapfreUserSecurityLibrary.Implementation;
using MPR.RestApiTemplate.Api.Middlewares.UserContext.Interfaces;

namespace MPR.RestApiTemplate.Api.Middlewares.UserContext
{
    internal class UserContextMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context, IUserContextInternal userContext)
        {
            userContext.UserId = GetUserId(context);
            userContext.IpAddress = GetClientIpAddress(context);
            userContext.NumUserId = GetNumUserId(userContext.UserId, userContext.IpAddress);
            userContext.TraceId = Guid.NewGuid().ToString();
            userContext.AppId = string.IsNullOrWhiteSpace(clsUtilities.GetAppID()) ? 0 : int.Parse(clsUtilities.GetAppID());
            userContext.UserAppRoles = clsApplication.GetUserApplicationRoles(userContext.AppId, userContext.NumUserId);
            userContext.HttpRequest = context.Request;

            await _next(context);
        }

        private static string GetClientIpAddress(HttpContext context)
        {
            // 1. Intenta obtener la IP del header X-Forwarded-For
            var xForwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(xForwardedFor))
            {
                // Puede contener varias IPs separadas por coma
                var ip = xForwardedFor.Split(',').Select(s => s.Trim()).FirstOrDefault();
                if (IPAddress.TryParse(ip, out var parsedIp))
                    return parsedIp.ToString();
            }

            // 2. Usa la IP remota de la conexión
            var remoteIp = context.Connection.RemoteIpAddress;
            if (remoteIp != null)
            {
                // Si es loopback IPv6, retorna 127.0.0.1
                if (IPAddress.IsLoopback(remoteIp))
                    return "127.0.0.1";
                // Siempre retorna la versión IPv4 si es posible
                return remoteIp.MapToIPv4().ToString();
            }

            // 3. Si no hay IP, retorna string vacío
            return string.Empty;
        }

        private static string GetUserId(HttpContext context)
        {
            var contextUser = context.User?.Identity?.Name ?? "Anonymous";
            var userId = contextUser;

            if (contextUser != "Anonymous" && contextUser.Contains('\\'))
            {
                var domainUserName = contextUser.Split('\\');
                userId = domainUserName[1];
            }

            // Use test user in config file if flag ADUserTestIsOn is set to Y
            var adUserTestIsOn = clsUtilities.GetConfigValue("ADUserTestIsOn");
            var adUserTest = clsUtilities.GetConfigValue("ADUserTest");
            userId = adUserTestIsOn == "Y" ? adUserTest : userId;

            return userId;
        }
    
        private static long GetNumUserId(string userId, string ipAddress)
        {
            var user = new clsUsers("fpereira", clsUsers.AccountTypes.ActiveDirectory, ipAddress);
            return user.NumUserID;
        } 
    }
}
