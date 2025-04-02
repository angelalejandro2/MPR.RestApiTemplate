
using Microsoft.Extensions.DependencyInjection;

namespace MPR.RestApiTemplate.Application.Services;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<AlphabeticalListOfProductsService>();
        services.AddScoped<CategoriesService>();
        services.AddScoped<CategorySalesFor1997Service>();
        services.AddScoped<CurrentProductListService>();
        services.AddScoped<CustomerAndSuppliersByCityService>();
        services.AddScoped<CustomerDemographicsService>();
        services.AddScoped<CustomersService>();
        services.AddScoped<EmployeesService>();
        services.AddScoped<InvoicesService>();
        services.AddScoped<OrderDetailsService>();
        services.AddScoped<OrderDetailsExtendedService>();
        services.AddScoped<OrdersService>();
        services.AddScoped<OrdersQryService>();
        services.AddScoped<OrderSubtotalsService>();
        services.AddScoped<ProductsService>();
        services.AddScoped<ProductsAboveAveragePriceService>();
        services.AddScoped<ProductSalesFor1997Service>();
        services.AddScoped<ProductsByCategoryService>();
        services.AddScoped<QuarterlyOrdersService>();
        services.AddScoped<RegionService>();
        services.AddScoped<SalesByCategoryService>();
        services.AddScoped<SalesTotalsByAmountService>();
        services.AddScoped<ShippersService>();
        services.AddScoped<SummaryOfSalesByQuarterService>();
        services.AddScoped<SummaryOfSalesByYearService>();
        services.AddScoped<SuppliersService>();
        services.AddScoped<TerritoriesService>();
        return services;
    }
}
