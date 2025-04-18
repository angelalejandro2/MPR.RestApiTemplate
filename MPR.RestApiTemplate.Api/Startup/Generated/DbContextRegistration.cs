// Autogenerated Code - Do not modify
// Implement custom logic in a partial class

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MPR.RestApiTemplate.Infrastructure.Context
{
    public partial class DbContextRegistration
    {
        private readonly IServiceCollection _services;
        private readonly IConfiguration _configuration;

        public DbContextRegistration(IServiceCollection services, IConfiguration configuration)
        {
            _services = services;
            _configuration = configuration;
        }

        public IServiceCollection RegisterDbContexts()
        {
            ConfigureDbContexts();
            return _services;
        }

        protected virtual void ConfigureDbContexts()
        {
            _services.AddDbContext<NorthwindContext>(options =>
                options.UseSqlServer(
                    _configuration.GetConnectionString("NorthwindConnection"),
                    optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(NorthwindContext).Assembly.FullName)
                )
            );
        }
    }

    public static class DbContextRegistrationExtensions
    {
        public static IServiceCollection AddInfrastructureDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            return new DbContextRegistration(services, configuration).RegisterDbContexts();
        }
    }
}
