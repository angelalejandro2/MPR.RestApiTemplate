
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MPR.RestApiTemplate.Application.DTOs;

namespace MPR.RestApiTemplate.Application.Mappings;

public static class AutoMapperRegistration
{
    public static IServiceCollection AddMappingProfiles(this IServiceCollection services)
    {
        services.AddSingleton<Profile, AlphabeticalListOfProductsMapping>();
        services.AddSingleton<Profile, CategoriesMapping>();
        services.AddSingleton<Profile, CategorySalesFor1997Mapping>();
        services.AddSingleton<Profile, CurrentProductListMapping>();
        services.AddSingleton<Profile, CustomerAndSuppliersByCityMapping>();
        services.AddSingleton<Profile, CustomerDemographicsMapping>();
        services.AddSingleton<Profile, CustomersMapping>();
        services.AddSingleton<Profile, EmployeesMapping>();
        services.AddSingleton<Profile, InvoicesMapping>();
        services.AddSingleton<Profile, OrderDetailsMapping>();
        services.AddSingleton<Profile, OrderDetailsExtendedMapping>();
        services.AddSingleton<Profile, OrdersMapping>();
        services.AddSingleton<Profile, OrdersQryMapping>();
        services.AddSingleton<Profile, OrderSubtotalsMapping>();
        services.AddSingleton<Profile, ProductsMapping>();
        services.AddSingleton<Profile, ProductsAboveAveragePriceMapping>();
        services.AddSingleton<Profile, ProductSalesFor1997Mapping>();
        services.AddSingleton<Profile, ProductsByCategoryMapping>();
        services.AddSingleton<Profile, QuarterlyOrdersMapping>();
        services.AddSingleton<Profile, RegionMapping>();
        services.AddSingleton<Profile, SalesByCategoryMapping>();
        services.AddSingleton<Profile, SalesTotalsByAmountMapping>();
        services.AddSingleton<Profile, ShippersMapping>();
        services.AddSingleton<Profile, SummaryOfSalesByQuarterMapping>();
        services.AddSingleton<Profile, SummaryOfSalesByYearMapping>();
        services.AddSingleton<Profile, SuppliersMapping>();
        services.AddSingleton<Profile, TerritoriesMapping>();
        
        services.AddAutoMapper(
            typeof(AlphabeticalListOfProductsMapping),
            typeof(CategoriesMapping),
            typeof(CategorySalesFor1997Mapping),
            typeof(CurrentProductListMapping),
            typeof(CustomerAndSuppliersByCityMapping),
            typeof(CustomerDemographicsMapping),
            typeof(CustomersMapping),
            typeof(EmployeesMapping),
            typeof(InvoicesMapping),
            typeof(OrderDetailsMapping),
            typeof(OrderDetailsExtendedMapping),
            typeof(OrdersMapping),
            typeof(OrdersQryMapping),
            typeof(OrderSubtotalsMapping),
            typeof(ProductsMapping),
            typeof(ProductsAboveAveragePriceMapping),
            typeof(ProductSalesFor1997Mapping),
            typeof(ProductsByCategoryMapping),
            typeof(QuarterlyOrdersMapping),
            typeof(RegionMapping),
            typeof(SalesByCategoryMapping),
            typeof(SalesTotalsByAmountMapping),
            typeof(ShippersMapping),
            typeof(SummaryOfSalesByQuarterMapping),
            typeof(SummaryOfSalesByYearMapping),
            typeof(SuppliersMapping),
            typeof(TerritoriesMapping)
        );

        return services;
    }
}