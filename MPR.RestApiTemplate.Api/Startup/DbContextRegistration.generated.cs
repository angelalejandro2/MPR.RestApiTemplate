
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MPR.RestApiTemplate.Infrastructure.Context;

public static class DbContextRegistration
{
    public static IServiceCollection AddInfrastructureDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DefaultDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(DefaultDbContext).Assembly.FullName)
            )
        );
        return services;
    }
}