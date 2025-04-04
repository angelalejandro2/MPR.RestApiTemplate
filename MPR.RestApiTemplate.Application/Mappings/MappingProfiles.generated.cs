using AutoMapper;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Domain.Entities;

namespace MPR.RestApiTemplate.Application.DTOs
{
	public class AlphabeticalListOfProductsMapping : Profile
	{
		public AlphabeticalListOfProductsMapping()
		{
			CreateMap<AlphabeticalListOfProducts, AlphabeticalListOfProductsDto>().ReverseMap();
			CreateMap<AlphabeticalListOfProducts, AlphabeticalListOfProductsCreateDto>().ReverseMap();
			CreateMap<AlphabeticalListOfProducts, AlphabeticalListOfProductsUpdateDto>().ReverseMap();
		}
	}
	public class CategoriesMapping : Profile
	{
		public CategoriesMapping()
		{
			CreateMap<Categories, CategoriesDto>().ReverseMap();
			CreateMap<Categories, CategoriesCreateDto>().ReverseMap();
			CreateMap<Categories, CategoriesUpdateDto>().ReverseMap();
		}
	}
	public class CategorySalesFor1997Mapping : Profile
	{
		public CategorySalesFor1997Mapping()
		{
			CreateMap<CategorySalesFor1997, CategorySalesFor1997Dto>().ReverseMap();
			CreateMap<CategorySalesFor1997, CategorySalesFor1997CreateDto>().ReverseMap();
			CreateMap<CategorySalesFor1997, CategorySalesFor1997UpdateDto>().ReverseMap();
		}
	}
	public class CurrentProductListMapping : Profile
	{
		public CurrentProductListMapping()
		{
			CreateMap<CurrentProductList, CurrentProductListDto>().ReverseMap();
			CreateMap<CurrentProductList, CurrentProductListCreateDto>().ReverseMap();
			CreateMap<CurrentProductList, CurrentProductListUpdateDto>().ReverseMap();
		}
	}
	public class CustomerAndSuppliersByCityMapping : Profile
	{
		public CustomerAndSuppliersByCityMapping()
		{
			CreateMap<CustomerAndSuppliersByCity, CustomerAndSuppliersByCityDto>().ReverseMap();
			CreateMap<CustomerAndSuppliersByCity, CustomerAndSuppliersByCityCreateDto>().ReverseMap();
			CreateMap<CustomerAndSuppliersByCity, CustomerAndSuppliersByCityUpdateDto>().ReverseMap();
		}
	}
	public class CustomerDemographicsMapping : Profile
	{
		public CustomerDemographicsMapping()
		{
			CreateMap<CustomerDemographics, CustomerDemographicsDto>().ReverseMap();
			CreateMap<CustomerDemographics, CustomerDemographicsCreateDto>().ReverseMap();
			CreateMap<CustomerDemographics, CustomerDemographicsUpdateDto>().ReverseMap();
		}
	}
	public class CustomersMapping : Profile
	{
		public CustomersMapping()
		{
			CreateMap<Customers, CustomersDto>().ReverseMap();
			CreateMap<Customers, CustomersCreateDto>().ReverseMap();
			CreateMap<Customers, CustomersUpdateDto>().ReverseMap();
		}
	}
	public class EmployeesMapping : Profile
	{
		public EmployeesMapping()
		{
			CreateMap<Employees, EmployeesDto>().ReverseMap();
			CreateMap<Employees, EmployeesCreateDto>().ReverseMap();
			CreateMap<Employees, EmployeesUpdateDto>().ReverseMap();
		}
	}
	public class InvoicesMapping : Profile
	{
		public InvoicesMapping()
		{
			CreateMap<Invoices, InvoicesDto>().ReverseMap();
			CreateMap<Invoices, InvoicesCreateDto>().ReverseMap();
			CreateMap<Invoices, InvoicesUpdateDto>().ReverseMap();
		}
	}
	public class OrderDetailsMapping : Profile
	{
		public OrderDetailsMapping()
		{
			CreateMap<OrderDetails, OrderDetailsDto>().ReverseMap();
			CreateMap<OrderDetails, OrderDetailsCreateDto>().ReverseMap();
			CreateMap<OrderDetails, OrderDetailsUpdateDto>().ReverseMap();
		}
	}
	public class OrderDetailsExtendedMapping : Profile
	{
		public OrderDetailsExtendedMapping()
		{
			CreateMap<OrderDetailsExtended, OrderDetailsExtendedDto>().ReverseMap();
			CreateMap<OrderDetailsExtended, OrderDetailsExtendedCreateDto>().ReverseMap();
			CreateMap<OrderDetailsExtended, OrderDetailsExtendedUpdateDto>().ReverseMap();
		}
	}
	public class OrdersMapping : Profile
	{
		public OrdersMapping()
		{
			CreateMap<Orders, OrdersDto>().ReverseMap();
			CreateMap<Orders, OrdersCreateDto>().ReverseMap();
			CreateMap<Orders, OrdersUpdateDto>().ReverseMap();
		}
	}
	public class OrdersQryMapping : Profile
	{
		public OrdersQryMapping()
		{
			CreateMap<OrdersQry, OrdersQryDto>().ReverseMap();
			CreateMap<OrdersQry, OrdersQryCreateDto>().ReverseMap();
			CreateMap<OrdersQry, OrdersQryUpdateDto>().ReverseMap();
		}
	}
	public class OrderSubtotalsMapping : Profile
	{
		public OrderSubtotalsMapping()
		{
			CreateMap<OrderSubtotals, OrderSubtotalsDto>().ReverseMap();
			CreateMap<OrderSubtotals, OrderSubtotalsCreateDto>().ReverseMap();
			CreateMap<OrderSubtotals, OrderSubtotalsUpdateDto>().ReverseMap();
		}
	}
	public class ProductsMapping : Profile
	{
		public ProductsMapping()
		{
			CreateMap<Products, ProductsDto>().ReverseMap();
			CreateMap<Products, ProductsCreateDto>().ReverseMap();
			CreateMap<Products, ProductsUpdateDto>().ReverseMap();
		}
	}
	public class ProductsAboveAveragePriceMapping : Profile
	{
		public ProductsAboveAveragePriceMapping()
		{
			CreateMap<ProductsAboveAveragePrice, ProductsAboveAveragePriceDto>().ReverseMap();
			CreateMap<ProductsAboveAveragePrice, ProductsAboveAveragePriceCreateDto>().ReverseMap();
			CreateMap<ProductsAboveAveragePrice, ProductsAboveAveragePriceUpdateDto>().ReverseMap();
		}
	}
	public class ProductSalesFor1997Mapping : Profile
	{
		public ProductSalesFor1997Mapping()
		{
			CreateMap<ProductSalesFor1997, ProductSalesFor1997Dto>().ReverseMap();
			CreateMap<ProductSalesFor1997, ProductSalesFor1997CreateDto>().ReverseMap();
			CreateMap<ProductSalesFor1997, ProductSalesFor1997UpdateDto>().ReverseMap();
		}
	}
	public class ProductsByCategoryMapping : Profile
	{
		public ProductsByCategoryMapping()
		{
			CreateMap<ProductsByCategory, ProductsByCategoryDto>().ReverseMap();
			CreateMap<ProductsByCategory, ProductsByCategoryCreateDto>().ReverseMap();
			CreateMap<ProductsByCategory, ProductsByCategoryUpdateDto>().ReverseMap();
		}
	}
	public class QuarterlyOrdersMapping : Profile
	{
		public QuarterlyOrdersMapping()
		{
			CreateMap<QuarterlyOrders, QuarterlyOrdersDto>().ReverseMap();
			CreateMap<QuarterlyOrders, QuarterlyOrdersCreateDto>().ReverseMap();
			CreateMap<QuarterlyOrders, QuarterlyOrdersUpdateDto>().ReverseMap();
		}
	}
	public class RegionMapping : Profile
	{
		public RegionMapping()
		{
			CreateMap<Region, RegionDto>().ReverseMap();
			CreateMap<Region, RegionCreateDto>().ReverseMap();
			CreateMap<Region, RegionUpdateDto>().ReverseMap();
		}
	}
	public class SalesByCategoryMapping : Profile
	{
		public SalesByCategoryMapping()
		{
			CreateMap<SalesByCategory, SalesByCategoryDto>().ReverseMap();
			CreateMap<SalesByCategory, SalesByCategoryCreateDto>().ReverseMap();
			CreateMap<SalesByCategory, SalesByCategoryUpdateDto>().ReverseMap();
		}
	}
	public class SalesTotalsByAmountMapping : Profile
	{
		public SalesTotalsByAmountMapping()
		{
			CreateMap<SalesTotalsByAmount, SalesTotalsByAmountDto>().ReverseMap();
			CreateMap<SalesTotalsByAmount, SalesTotalsByAmountCreateDto>().ReverseMap();
			CreateMap<SalesTotalsByAmount, SalesTotalsByAmountUpdateDto>().ReverseMap();
		}
	}
	public class ShippersMapping : Profile
	{
		public ShippersMapping()
		{
			CreateMap<Shippers, ShippersDto>().ReverseMap();
			CreateMap<Shippers, ShippersCreateDto>().ReverseMap();
			CreateMap<Shippers, ShippersUpdateDto>().ReverseMap();
		}
	}
	public class SummaryOfSalesByQuarterMapping : Profile
	{
		public SummaryOfSalesByQuarterMapping()
		{
			CreateMap<SummaryOfSalesByQuarter, SummaryOfSalesByQuarterDto>().ReverseMap();
			CreateMap<SummaryOfSalesByQuarter, SummaryOfSalesByQuarterCreateDto>().ReverseMap();
			CreateMap<SummaryOfSalesByQuarter, SummaryOfSalesByQuarterUpdateDto>().ReverseMap();
		}
	}
	public class SummaryOfSalesByYearMapping : Profile
	{
		public SummaryOfSalesByYearMapping()
		{
			CreateMap<SummaryOfSalesByYear, SummaryOfSalesByYearDto>().ReverseMap();
			CreateMap<SummaryOfSalesByYear, SummaryOfSalesByYearCreateDto>().ReverseMap();
			CreateMap<SummaryOfSalesByYear, SummaryOfSalesByYearUpdateDto>().ReverseMap();
		}
	}
	public class SuppliersMapping : Profile
	{
		public SuppliersMapping()
		{
			CreateMap<Suppliers, SuppliersDto>().ReverseMap();
			CreateMap<Suppliers, SuppliersCreateDto>().ReverseMap();
			CreateMap<Suppliers, SuppliersUpdateDto>().ReverseMap();
		}
	}
	public class TerritoriesMapping : Profile
	{
		public TerritoriesMapping()
		{
			CreateMap<Territories, TerritoriesDto>().ReverseMap();
			CreateMap<Territories, TerritoriesCreateDto>().ReverseMap();
			CreateMap<Territories, TerritoriesUpdateDto>().ReverseMap();
		}
	}
}