using MPR.RestApiTemplate.Domain.Interfaces.Repositories;

namespace MPR.RestApiTemplate.Domain.Interfaces
{
	public partial interface IUnitOfWork : IDisposable
	{
		IAlphabetical_list_of_productRepository Alphabetical_list_of_productRepository { get; }
		ICategoryRepository CategoryRepository { get; }
		ICategory_Sales_for_1997Repository Category_Sales_for_1997Repository { get; }
		ICurrent_Product_ListRepository Current_Product_ListRepository { get; }
		ICustomerRepository CustomerRepository { get; }
		ICustomerDemographicRepository CustomerDemographicRepository { get; }
		ICustomer_and_Suppliers_by_CityRepository Customer_and_Suppliers_by_CityRepository { get; }
		IEmployeeRepository EmployeeRepository { get; }
		IInvoiceRepository InvoiceRepository { get; }
		IOrderRepository OrderRepository { get; }
		IOrders_QryRepository Orders_QryRepository { get; }
		IOrder_DetailRepository Order_DetailRepository { get; }
		IOrder_Details_ExtendedRepository Order_Details_ExtendedRepository { get; }
		IOrder_SubtotalRepository Order_SubtotalRepository { get; }
		IProductRepository ProductRepository { get; }
		IProducts_Above_Average_PriceRepository Products_Above_Average_PriceRepository { get; }
		IProducts_by_CategoryRepository Products_by_CategoryRepository { get; }
		IProduct_Sales_for_1997Repository Product_Sales_for_1997Repository { get; }
		IQuarterly_OrderRepository Quarterly_OrderRepository { get; }
		IRegionRepository RegionRepository { get; }
		ISales_by_CategoryRepository Sales_by_CategoryRepository { get; }
		ISales_Totals_by_AmountRepository Sales_Totals_by_AmountRepository { get; }
		IShipperRepository ShipperRepository { get; }
		ISummary_of_Sales_by_QuarterRepository Summary_of_Sales_by_QuarterRepository { get; }
		ISummary_of_Sales_by_YearRepository Summary_of_Sales_by_YearRepository { get; }
		ISupplierRepository SupplierRepository { get; }
		ITerritoryRepository TerritoryRepository { get; }
		INorthwindContextSpRepository NorthwindContextSpRepository { get; }

		Task<int> SaveChangesAsync();
	}
}