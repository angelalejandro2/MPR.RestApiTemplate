using MPR.RestApiTemplate.Api.Middlewares.UserContext.Interfaces;
using MPR.RestApiTemplate.Api.Middlewares.UserContext.Models;

namespace MPR.RestApiTemplate.Api.Middlewares.UserContext.Extensions
{
    public static class UserContextExtensions
    {
        public static IServiceCollection AddUserContext(this IServiceCollection services)
        {
            services.AddScoped<UserContextModel>();
            services.AddScoped<IUserContextInternal>(sp => sp.GetRequiredService<UserContextModel>());
            services.AddScoped<IUserContext>(sp => sp.GetRequiredService<UserContextModel>());
            return services;
        }

        public static IApplicationBuilder UseUserContext(this IApplicationBuilder app)
        {
            return app.UseMiddleware<UserContextMiddleware>();
        }
    }
}
