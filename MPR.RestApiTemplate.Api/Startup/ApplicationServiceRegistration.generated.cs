
using Microsoft.Extensions.DependencyInjection;

namespace MPR.RestApiTemplate.Application.Services;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<CustomerService>();
        services.AddScoped<CustomerTypeService>();
        return services;
    }
}
