using MPR.RestApiTemplate.Domain.Entities;

namespace MPR.RestApiTemplate.Domain.Interfaces.Repositories
{
	public interface IAlphabeticalListOfProductsRepository : IGenericRepository<AlphabeticalListOfProducts>	{ }
	public interface ICategoriesRepository : IGenericRepository<Categories>	{ }
	public interface ICategorySalesFor1997Repository : IGenericRepository<CategorySalesFor1997>	{ }
	public interface ICurrentProductListRepository : IGenericRepository<CurrentProductList>	{ }
	public interface ICustomerAndSuppliersByCityRepository : IGenericRepository<CustomerAndSuppliersByCity>	{ }
	public interface ICustomerDemographicsRepository : IGenericRepository<CustomerDemographics>	{ }
	public interface ICustomersRepository : IGenericRepository<Customers>	{ }
	public interface IEmployeesRepository : IGenericRepository<Employees>	{ }
	public interface IInvoicesRepository : IGenericRepository<Invoices>	{ }
	public interface IOrderDetailsRepository : IGenericRepository<OrderDetails>	{ }
	public interface IOrderDetailsExtendedRepository : IGenericRepository<OrderDetailsExtended>	{ }
	public interface IOrdersRepository : IGenericRepository<Orders>	{ }
	public interface IOrdersQryRepository : IGenericRepository<OrdersQry>	{ }
	public interface IOrderSubtotalsRepository : IGenericRepository<OrderSubtotals>	{ }
	public interface IProductsRepository : IGenericRepository<Products>	{ }
	public interface IProductsAboveAveragePriceRepository : IGenericRepository<ProductsAboveAveragePrice>	{ }
	public interface IProductSalesFor1997Repository : IGenericRepository<ProductSalesFor1997>	{ }
	public interface IProductsByCategoryRepository : IGenericRepository<ProductsByCategory>	{ }
	public interface IQuarterlyOrdersRepository : IGenericRepository<QuarterlyOrders>	{ }
	public interface IRegionRepository : IGenericRepository<Region>	{ }
	public interface ISalesByCategoryRepository : IGenericRepository<SalesByCategory>	{ }
	public interface ISalesTotalsByAmountRepository : IGenericRepository<SalesTotalsByAmount>	{ }
	public interface IShippersRepository : IGenericRepository<Shippers>	{ }
	public interface ISummaryOfSalesByQuarterRepository : IGenericRepository<SummaryOfSalesByQuarter>	{ }
	public interface ISummaryOfSalesByYearRepository : IGenericRepository<SummaryOfSalesByYear>	{ }
	public interface ISuppliersRepository : IGenericRepository<Suppliers>	{ }
	public interface ITerritoriesRepository : IGenericRepository<Territories>	{ }
}