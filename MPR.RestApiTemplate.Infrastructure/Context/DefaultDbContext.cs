using Microsoft.EntityFrameworkCore;
using MPR.RestApiTemplate.Domain.Entities;
using MPR.RestApiTemplate.Infrastructure.Attributes;

namespace MPR.RestApiTemplate.Infrastructure.Context
{
    [DbProvider(DbProvider.SqlServer)]
    public class DefaultDbContext(DbContextOptions<DefaultDbContext> options) : DbContext(options)
    {
        // TODO: Define the DbSet properties here
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerType> CustomerTypes { get; set; }
    }
}