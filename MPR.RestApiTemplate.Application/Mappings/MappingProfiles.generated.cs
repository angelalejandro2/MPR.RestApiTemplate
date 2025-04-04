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
		}
	}
	public class CurrentProductListMapping : Profile
	{
		public CurrentProductListMapping()
		{
			CreateMap<CurrentProductList, CurrentProductListDto>().ReverseMap();
		}
	}
	public class CustomerAndSuppliersByCityMapping : Profile
	{
		public CustomerAndSuppliersByCityMapping()
		{
			CreateMap<CustomerAndSuppliersByCity, CustomerAndSuppliersByCityDto>().ReverseMap();
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
		}
	}
	public class OrderSubtotalsMapping : Profile
	{
		public OrderSubtotalsMapping()
		{
			CreateMap<OrderSubtotals, OrderSubtotalsDto>().ReverseMap();
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
		}
	}
	public class ProductSalesFor1997Mapping : Profile
	{
		public ProductSalesFor1997Mapping()
		{
			CreateMap<ProductSalesFor1997, ProductSalesFor1997Dto>().ReverseMap();
		}
	}
	public class ProductsByCategoryMapping : Profile
	{
		public ProductsByCategoryMapping()
		{
			CreateMap<ProductsByCategory, ProductsByCategoryDto>().ReverseMap();
		}
	}
	public class QuarterlyOrdersMapping : Profile
	{
		public QuarterlyOrdersMapping()
		{
			CreateMap<QuarterlyOrders, QuarterlyOrdersDto>().ReverseMap();
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
		}
	}
	public class SalesTotalsByAmountMapping : Profile
	{
		public SalesTotalsByAmountMapping()
		{
			CreateMap<SalesTotalsByAmount, SalesTotalsByAmountDto>().ReverseMap();
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
		}
	}
	public class SummaryOfSalesByYearMapping : Profile
	{
		public SummaryOfSalesByYearMapping()
		{
			CreateMap<SummaryOfSalesByYear, SummaryOfSalesByYearDto>().ReverseMap();
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