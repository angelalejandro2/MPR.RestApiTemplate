
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MPR.RestApiTemplate.Application.Models;

namespace MPR.RestApiTemplate.Application.Mappings;

public static class AutoMapperRegistration
{
    public static IServiceCollection AddMappingProfiles(this IServiceCollection services)
    {
        services.AddSingleton<Profile, CustomerMapping>();
        services.AddSingleton<Profile, CustomerTypeMapping>();
        
        services.AddAutoMapper(
            typeof(CustomerMapping),
            typeof(CustomerTypeMapping)
        );

        return services;
    }
}