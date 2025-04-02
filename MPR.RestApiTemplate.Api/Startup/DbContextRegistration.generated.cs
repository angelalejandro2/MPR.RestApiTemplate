using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MPR.RestApiTemplate.Infrastructure.Context;

public static class DbContextRegistration
{
    public static IServiceCollection AddInfrastructureDbContexts(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<NorthwindContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("NorthwindConnection"),
                optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(NorthwindContext).Assembly.FullName)
            )
        );
        return services;
    }
}