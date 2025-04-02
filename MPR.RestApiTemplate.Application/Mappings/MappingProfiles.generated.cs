using AutoMapper;
using MPR.RestApiTemplate.Application.Models;
using MPR.RestApiTemplate.Domain.Entities;

namespace MPR.RestApiTemplate.Application.Models
{
	public class AlphabeticalListOfProductsMapping : Profile
	{
		public AlphabeticalListOfProductsMapping()
		{
			CreateMap<AlphabeticalListOfProducts, AlphabeticalListOfProductsModel>().ReverseMap();
		}
	}
	public class CategoriesMapping : Profile
	{
		public CategoriesMapping()
		{
			CreateMap<Categories, CategoriesModel>().ReverseMap();
		}
	}
	public class CategorySalesFor1997Mapping : Profile
	{
		public CategorySalesFor1997Mapping()
		{
			CreateMap<CategorySalesFor1997, CategorySalesFor1997Model>().ReverseMap();
		}
	}
	public class CurrentProductListMapping : Profile
	{
		public CurrentProductListMapping()
		{
			CreateMap<CurrentProductList, CurrentProductListModel>().ReverseMap();
		}
	}
	public class CustomerAndSuppliersByCityMapping : Profile
	{
		public CustomerAndSuppliersByCityMapping()
		{
			CreateMap<CustomerAndSuppliersByCity, CustomerAndSuppliersByCityModel>().ReverseMap();
		}
	}
	public class CustomerDemographicsMapping : Profile
	{
		public CustomerDemographicsMapping()
		{
			CreateMap<CustomerDemographics, CustomerDemographicsModel>().ReverseMap();
		}
	}
	public class CustomersMapping : Profile
	{
		public CustomersMapping()
		{
			CreateMap<Customers, CustomersModel>().ReverseMap();
		}
	}
	public class EmployeesMapping : Profile
	{
		public EmployeesMapping()
		{
			CreateMap<Employees, EmployeesModel>().ReverseMap();
		}
	}
	public class InvoicesMapping : Profile
	{
		public InvoicesMapping()
		{
			CreateMap<Invoices, InvoicesModel>().ReverseMap();
		}
	}
	public class OrderDetailsMapping : Profile
	{
		public OrderDetailsMapping()
		{
			CreateMap<OrderDetails, OrderDetailsModel>().ReverseMap();
		}
	}
	public class OrderDetailsExtendedMapping : Profile
	{
		public OrderDetailsExtendedMapping()
		{
			CreateMap<OrderDetailsExtended, OrderDetailsExtendedModel>().ReverseMap();
		}
	}
	public class OrdersMapping : Profile
	{
		public OrdersMapping()
		{
			CreateMap<Orders, OrdersModel>().ReverseMap();
		}
	}
	public class OrdersQryMapping : Profile
	{
		public OrdersQryMapping()
		{
			CreateMap<OrdersQry, OrdersQryModel>().ReverseMap();
		}
	}
	public class OrderSubtotalsMapping : Profile
	{
		public OrderSubtotalsMapping()
		{
			CreateMap<OrderSubtotals, OrderSubtotalsModel>().ReverseMap();
		}
	}
	public class ProductsMapping : Profile
	{
		public ProductsMapping()
		{
			CreateMap<Products, ProductsModel>().ReverseMap();
		}
	}
	public class ProductsAboveAveragePriceMapping : Profile
	{
		public ProductsAboveAveragePriceMapping()
		{
			CreateMap<ProductsAboveAveragePrice, ProductsAboveAveragePriceModel>().ReverseMap();
		}
	}
	public class ProductSalesFor1997Mapping : Profile
	{
		public ProductSalesFor1997Mapping()
		{
			CreateMap<ProductSalesFor1997, ProductSalesFor1997Model>().ReverseMap();
		}
	}
	public class ProductsByCategoryMapping : Profile
	{
		public ProductsByCategoryMapping()
		{
			CreateMap<ProductsByCategory, ProductsByCategoryModel>().ReverseMap();
		}
	}
	public class QuarterlyOrdersMapping : Profile
	{
		public QuarterlyOrdersMapping()
		{
			CreateMap<QuarterlyOrders, QuarterlyOrdersModel>().ReverseMap();
		}
	}
	public class RegionMapping : Profile
	{
		public RegionMapping()
		{
			CreateMap<Region, RegionModel>().ReverseMap();
		}
	}
	public class SalesByCategoryMapping : Profile
	{
		public SalesByCategoryMapping()
		{
			CreateMap<SalesByCategory, SalesByCategoryModel>().ReverseMap();
		}
	}
	public class SalesTotalsByAmountMapping : Profile
	{
		public SalesTotalsByAmountMapping()
		{
			CreateMap<SalesTotalsByAmount, SalesTotalsByAmountModel>().ReverseMap();
		}
	}
	public class ShippersMapping : Profile
	{
		public ShippersMapping()
		{
			CreateMap<Shippers, ShippersModel>().ReverseMap();
		}
	}
	public class SummaryOfSalesByQuarterMapping : Profile
	{
		public SummaryOfSalesByQuarterMapping()
		{
			CreateMap<SummaryOfSalesByQuarter, SummaryOfSalesByQuarterModel>().ReverseMap();
		}
	}
	public class SummaryOfSalesByYearMapping : Profile
	{
		public SummaryOfSalesByYearMapping()
		{
			CreateMap<SummaryOfSalesByYear, SummaryOfSalesByYearModel>().ReverseMap();
		}
	}
	public class SuppliersMapping : Profile
	{
		public SuppliersMapping()
		{
			CreateMap<Suppliers, SuppliersModel>().ReverseMap();
		}
	}
	public class TerritoriesMapping : Profile
	{
		public TerritoriesMapping()
		{
			CreateMap<Territories, TerritoriesModel>().ReverseMap();
		}
	}
}