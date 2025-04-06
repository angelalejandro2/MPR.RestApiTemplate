using MPR.RestApiTemplate.Domain.Interfaces.Repositories;

namespace MPR.RestApiTemplate.Domain.Interfaces
{
	public partial interface IUnitOfWork : IDisposable
	{
		IAlphabeticalListOfProductsRepository AlphabeticalListOfProductsRepository { get; }
		ICategoriesRepository CategoriesRepository { get; }
		ICategorySalesFor1997Repository CategorySalesFor1997Repository { get; }
		ICurrentProductListRepository CurrentProductListRepository { get; }
		ICustomerAndSuppliersByCityRepository CustomerAndSuppliersByCityRepository { get; }
		ICustomerDemographicsRepository CustomerDemographicsRepository { get; }
		ICustomersRepository CustomersRepository { get; }
		IEmployeesRepository EmployeesRepository { get; }
		IInvoicesRepository InvoicesRepository { get; }
		IOrderDetailsRepository OrderDetailsRepository { get; }
		IOrderDetailsExtendedRepository OrderDetailsExtendedRepository { get; }
		IOrdersRepository OrdersRepository { get; }
		IOrdersQryRepository OrdersQryRepository { get; }
		IOrderSubtotalsRepository OrderSubtotalsRepository { get; }
		IProductsRepository ProductsRepository { get; }
		IProductsAboveAveragePriceRepository ProductsAboveAveragePriceRepository { get; }
		IProductSalesFor1997Repository ProductSalesFor1997Repository { get; }
		IProductsByCategoryRepository ProductsByCategoryRepository { get; }
		IQuarterlyOrdersRepository QuarterlyOrdersRepository { get; }
		IRegionRepository RegionRepository { get; }
		ISalesByCategoryRepository SalesByCategoryRepository { get; }
		ISalesTotalsByAmountRepository SalesTotalsByAmountRepository { get; }
		IShippersRepository ShippersRepository { get; }
		ISummaryOfSalesByQuarterRepository SummaryOfSalesByQuarterRepository { get; }
		ISummaryOfSalesByYearRepository SummaryOfSalesByYearRepository { get; }
		ISuppliersRepository SuppliersRepository { get; }
		ITerritoriesRepository TerritoriesRepository { get; }

		Task<int> SaveChangesAsync();
	}
}