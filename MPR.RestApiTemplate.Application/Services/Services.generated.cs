using AutoMapper;
using MPR.RestApiTemplate.Application.Models;
using MPR.RestApiTemplate.Domain.Entities;
using MPR.RestApiTemplate.Domain.Interfaces;

namespace MPR.RestApiTemplate.Application.Services
{
	public partial class AlphabeticalListOfProductsService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public AlphabeticalListOfProductsService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public virtual async Task<IEnumerable<AlphabeticalListOfProductsModel>> GetAllAsync()
		{
			var entities = await _unitOfWork.AlphabeticalListOfProductsRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<AlphabeticalListOfProductsModel>>(entities);
		}

	}
	public partial class CategoriesService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CategoriesService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public virtual async Task<IEnumerable<CategoriesModel>> GetAllAsync()
		{
			var entities = await _unitOfWork.CategoriesRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<CategoriesModel>>(entities);
		}

		public virtual async Task<CategoriesModel> GetByIdAsync(int categoryId)
		{
			var entity = await _unitOfWork.CategoriesRepository.GetByIdAsync(categoryId);
			return _mapper.Map<CategoriesModel>(entity);
		}

		public virtual async Task DeleteAsync(int categoryId)
		{
			await _unitOfWork.CategoriesRepository.DeleteAsync(categoryId);
			await _unitOfWork.SaveChangesAsync();
		}

		public virtual async Task<CategoriesModel> AddAsync(CategoriesModel model)
		{
			var entity = _mapper.Map<Categories>(model);
			entity = await _unitOfWork.CategoriesRepository.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<CategoriesModel>(entity);
		}

		public virtual async Task<CategoriesModel> UpdateAsync(CategoriesModel model)
		{
			var entity = _mapper.Map<Categories>(model);
			entity = await _unitOfWork.CategoriesRepository.UpdateAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<CategoriesModel>(entity);
		}
	}
	public partial class CategorySalesFor1997Service
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CategorySalesFor1997Service(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public virtual async Task<IEnumerable<CategorySalesFor1997Model>> GetAllAsync()
		{
			var entities = await _unitOfWork.CategorySalesFor1997Repository.GetAllAsync();
			return _mapper.Map<IEnumerable<CategorySalesFor1997Model>>(entities);
		}

	}
	public partial class CurrentProductListService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CurrentProductListService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public virtual async Task<IEnumerable<CurrentProductListModel>> GetAllAsync()
		{
			var entities = await _unitOfWork.CurrentProductListRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<CurrentProductListModel>>(entities);
		}

	}
	public partial class CustomerAndSuppliersByCityService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CustomerAndSuppliersByCityService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public virtual async Task<IEnumerable<CustomerAndSuppliersByCityModel>> GetAllAsync()
		{
			var entities = await _unitOfWork.CustomerAndSuppliersByCityRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<CustomerAndSuppliersByCityModel>>(entities);
		}

	}
	public partial class CustomerDemographicsService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CustomerDemographicsService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public virtual async Task<IEnumerable<CustomerDemographicsModel>> GetAllAsync()
		{
			var entities = await _unitOfWork.CustomerDemographicsRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<CustomerDemographicsModel>>(entities);
		}

		public virtual async Task<CustomerDemographicsModel> GetByIdAsync(string customerTypeId)
		{
			var entity = await _unitOfWork.CustomerDemographicsRepository.GetByIdAsync(customerTypeId);
			return _mapper.Map<CustomerDemographicsModel>(entity);
		}

		public virtual async Task DeleteAsync(string customerTypeId)
		{
			await _unitOfWork.CustomerDemographicsRepository.DeleteAsync(customerTypeId);
			await _unitOfWork.SaveChangesAsync();
		}

		public virtual async Task<CustomerDemographicsModel> AddAsync(CustomerDemographicsModel model)
		{
			var entity = _mapper.Map<CustomerDemographics>(model);
			entity = await _unitOfWork.CustomerDemographicsRepository.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<CustomerDemographicsModel>(entity);
		}

		public virtual async Task<CustomerDemographicsModel> UpdateAsync(CustomerDemographicsModel model)
		{
			var entity = _mapper.Map<CustomerDemographics>(model);
			entity = await _unitOfWork.CustomerDemographicsRepository.UpdateAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<CustomerDemographicsModel>(entity);
		}
	}
	public partial class CustomersService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CustomersService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public virtual async Task<IEnumerable<CustomersModel>> GetAllAsync()
		{
			var entities = await _unitOfWork.CustomersRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<CustomersModel>>(entities);
		}

		public virtual async Task<CustomersModel> GetByIdAsync(string customerId)
		{
			var entity = await _unitOfWork.CustomersRepository.GetByIdAsync(customerId);
			return _mapper.Map<CustomersModel>(entity);
		}

		public virtual async Task DeleteAsync(string customerId)
		{
			await _unitOfWork.CustomersRepository.DeleteAsync(customerId);
			await _unitOfWork.SaveChangesAsync();
		}

		public virtual async Task<CustomersModel> AddAsync(CustomersModel model)
		{
			var entity = _mapper.Map<Customers>(model);
			entity = await _unitOfWork.CustomersRepository.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<CustomersModel>(entity);
		}

		public virtual async Task<CustomersModel> UpdateAsync(CustomersModel model)
		{
			var entity = _mapper.Map<Customers>(model);
			entity = await _unitOfWork.CustomersRepository.UpdateAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<CustomersModel>(entity);
		}
	}
	public partial class EmployeesService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public EmployeesService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public virtual async Task<IEnumerable<EmployeesModel>> GetAllAsync()
		{
			var entities = await _unitOfWork.EmployeesRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<EmployeesModel>>(entities);
		}

		public virtual async Task<EmployeesModel> GetByIdAsync(int employeeId)
		{
			var entity = await _unitOfWork.EmployeesRepository.GetByIdAsync(employeeId);
			return _mapper.Map<EmployeesModel>(entity);
		}

		public virtual async Task DeleteAsync(int employeeId)
		{
			await _unitOfWork.EmployeesRepository.DeleteAsync(employeeId);
			await _unitOfWork.SaveChangesAsync();
		}

		public virtual async Task<EmployeesModel> AddAsync(EmployeesModel model)
		{
			var entity = _mapper.Map<Employees>(model);
			entity = await _unitOfWork.EmployeesRepository.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<EmployeesModel>(entity);
		}

		public virtual async Task<EmployeesModel> UpdateAsync(EmployeesModel model)
		{
			var entity = _mapper.Map<Employees>(model);
			entity = await _unitOfWork.EmployeesRepository.UpdateAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<EmployeesModel>(entity);
		}
	}
	public partial class InvoicesService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public InvoicesService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public virtual async Task<IEnumerable<InvoicesModel>> GetAllAsync()
		{
			var entities = await _unitOfWork.InvoicesRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<InvoicesModel>>(entities);
		}

	}
	public partial class OrderDetailsService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public OrderDetailsService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public virtual async Task<IEnumerable<OrderDetailsModel>> GetAllAsync()
		{
			var entities = await _unitOfWork.OrderDetailsRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<OrderDetailsModel>>(entities);
		}

		public virtual async Task<OrderDetailsModel> GetByIdAsync(int orderId, int productId)
		{
			var entity = await _unitOfWork.OrderDetailsRepository.GetByIdAsync(orderId, productId);
			return _mapper.Map<OrderDetailsModel>(entity);
		}

		public virtual async Task DeleteAsync(int orderId, int productId)
		{
			await _unitOfWork.OrderDetailsRepository.DeleteAsync(orderId, productId);
			await _unitOfWork.SaveChangesAsync();
		}

		public virtual async Task<OrderDetailsModel> AddAsync(OrderDetailsModel model)
		{
			var entity = _mapper.Map<OrderDetails>(model);
			entity = await _unitOfWork.OrderDetailsRepository.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<OrderDetailsModel>(entity);
		}

		public virtual async Task<OrderDetailsModel> UpdateAsync(OrderDetailsModel model)
		{
			var entity = _mapper.Map<OrderDetails>(model);
			entity = await _unitOfWork.OrderDetailsRepository.UpdateAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<OrderDetailsModel>(entity);
		}
	}
	public partial class OrderDetailsExtendedService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public OrderDetailsExtendedService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public virtual async Task<IEnumerable<OrderDetailsExtendedModel>> GetAllAsync()
		{
			var entities = await _unitOfWork.OrderDetailsExtendedRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<OrderDetailsExtendedModel>>(entities);
		}

	}
	public partial class OrdersService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public OrdersService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public virtual async Task<IEnumerable<OrdersModel>> GetAllAsync()
		{
			var entities = await _unitOfWork.OrdersRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<OrdersModel>>(entities);
		}

		public virtual async Task<OrdersModel> GetByIdAsync(int orderId)
		{
			var entity = await _unitOfWork.OrdersRepository.GetByIdAsync(orderId);
			return _mapper.Map<OrdersModel>(entity);
		}

		public virtual async Task DeleteAsync(int orderId)
		{
			await _unitOfWork.OrdersRepository.DeleteAsync(orderId);
			await _unitOfWork.SaveChangesAsync();
		}

		public virtual async Task<OrdersModel> AddAsync(OrdersModel model)
		{
			var entity = _mapper.Map<Orders>(model);
			entity = await _unitOfWork.OrdersRepository.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<OrdersModel>(entity);
		}

		public virtual async Task<OrdersModel> UpdateAsync(OrdersModel model)
		{
			var entity = _mapper.Map<Orders>(model);
			entity = await _unitOfWork.OrdersRepository.UpdateAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<OrdersModel>(entity);
		}
	}
	public partial class OrdersQryService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public OrdersQryService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public virtual async Task<IEnumerable<OrdersQryModel>> GetAllAsync()
		{
			var entities = await _unitOfWork.OrdersQryRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<OrdersQryModel>>(entities);
		}

	}
	public partial class OrderSubtotalsService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public OrderSubtotalsService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public virtual async Task<IEnumerable<OrderSubtotalsModel>> GetAllAsync()
		{
			var entities = await _unitOfWork.OrderSubtotalsRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<OrderSubtotalsModel>>(entities);
		}

	}
	public partial class ProductsService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ProductsService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public virtual async Task<IEnumerable<ProductsModel>> GetAllAsync()
		{
			var entities = await _unitOfWork.ProductsRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<ProductsModel>>(entities);
		}

		public virtual async Task<ProductsModel> GetByIdAsync(int productId)
		{
			var entity = await _unitOfWork.ProductsRepository.GetByIdAsync(productId);
			return _mapper.Map<ProductsModel>(entity);
		}

		public virtual async Task DeleteAsync(int productId)
		{
			await _unitOfWork.ProductsRepository.DeleteAsync(productId);
			await _unitOfWork.SaveChangesAsync();
		}

		public virtual async Task<ProductsModel> AddAsync(ProductsModel model)
		{
			var entity = _mapper.Map<Products>(model);
			entity = await _unitOfWork.ProductsRepository.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<ProductsModel>(entity);
		}

		public virtual async Task<ProductsModel> UpdateAsync(ProductsModel model)
		{
			var entity = _mapper.Map<Products>(model);
			entity = await _unitOfWork.ProductsRepository.UpdateAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<ProductsModel>(entity);
		}
	}
	public partial class ProductsAboveAveragePriceService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ProductsAboveAveragePriceService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public virtual async Task<IEnumerable<ProductsAboveAveragePriceModel>> GetAllAsync()
		{
			var entities = await _unitOfWork.ProductsAboveAveragePriceRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<ProductsAboveAveragePriceModel>>(entities);
		}

	}
	public partial class ProductSalesFor1997Service
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ProductSalesFor1997Service(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public virtual async Task<IEnumerable<ProductSalesFor1997Model>> GetAllAsync()
		{
			var entities = await _unitOfWork.ProductSalesFor1997Repository.GetAllAsync();
			return _mapper.Map<IEnumerable<ProductSalesFor1997Model>>(entities);
		}

	}
	public partial class ProductsByCategoryService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ProductsByCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public virtual async Task<IEnumerable<ProductsByCategoryModel>> GetAllAsync()
		{
			var entities = await _unitOfWork.ProductsByCategoryRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<ProductsByCategoryModel>>(entities);
		}

	}
	public partial class QuarterlyOrdersService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public QuarterlyOrdersService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public virtual async Task<IEnumerable<QuarterlyOrdersModel>> GetAllAsync()
		{
			var entities = await _unitOfWork.QuarterlyOrdersRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<QuarterlyOrdersModel>>(entities);
		}

	}
	public partial class RegionService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public RegionService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public virtual async Task<IEnumerable<RegionModel>> GetAllAsync()
		{
			var entities = await _unitOfWork.RegionRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<RegionModel>>(entities);
		}

		public virtual async Task<RegionModel> GetByIdAsync(int regionId)
		{
			var entity = await _unitOfWork.RegionRepository.GetByIdAsync(regionId);
			return _mapper.Map<RegionModel>(entity);
		}

		public virtual async Task DeleteAsync(int regionId)
		{
			await _unitOfWork.RegionRepository.DeleteAsync(regionId);
			await _unitOfWork.SaveChangesAsync();
		}

		public virtual async Task<RegionModel> AddAsync(RegionModel model)
		{
			var entity = _mapper.Map<Region>(model);
			entity = await _unitOfWork.RegionRepository.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<RegionModel>(entity);
		}

		public virtual async Task<RegionModel> UpdateAsync(RegionModel model)
		{
			var entity = _mapper.Map<Region>(model);
			entity = await _unitOfWork.RegionRepository.UpdateAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<RegionModel>(entity);
		}
	}
	public partial class SalesByCategoryService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public SalesByCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public virtual async Task<IEnumerable<SalesByCategoryModel>> GetAllAsync()
		{
			var entities = await _unitOfWork.SalesByCategoryRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<SalesByCategoryModel>>(entities);
		}

	}
	public partial class SalesTotalsByAmountService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public SalesTotalsByAmountService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public virtual async Task<IEnumerable<SalesTotalsByAmountModel>> GetAllAsync()
		{
			var entities = await _unitOfWork.SalesTotalsByAmountRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<SalesTotalsByAmountModel>>(entities);
		}

	}
	public partial class ShippersService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ShippersService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public virtual async Task<IEnumerable<ShippersModel>> GetAllAsync()
		{
			var entities = await _unitOfWork.ShippersRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<ShippersModel>>(entities);
		}

		public virtual async Task<ShippersModel> GetByIdAsync(int shipperId)
		{
			var entity = await _unitOfWork.ShippersRepository.GetByIdAsync(shipperId);
			return _mapper.Map<ShippersModel>(entity);
		}

		public virtual async Task DeleteAsync(int shipperId)
		{
			await _unitOfWork.ShippersRepository.DeleteAsync(shipperId);
			await _unitOfWork.SaveChangesAsync();
		}

		public virtual async Task<ShippersModel> AddAsync(ShippersModel model)
		{
			var entity = _mapper.Map<Shippers>(model);
			entity = await _unitOfWork.ShippersRepository.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<ShippersModel>(entity);
		}

		public virtual async Task<ShippersModel> UpdateAsync(ShippersModel model)
		{
			var entity = _mapper.Map<Shippers>(model);
			entity = await _unitOfWork.ShippersRepository.UpdateAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<ShippersModel>(entity);
		}
	}
	public partial class SummaryOfSalesByQuarterService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public SummaryOfSalesByQuarterService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public virtual async Task<IEnumerable<SummaryOfSalesByQuarterModel>> GetAllAsync()
		{
			var entities = await _unitOfWork.SummaryOfSalesByQuarterRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<SummaryOfSalesByQuarterModel>>(entities);
		}

	}
	public partial class SummaryOfSalesByYearService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public SummaryOfSalesByYearService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public virtual async Task<IEnumerable<SummaryOfSalesByYearModel>> GetAllAsync()
		{
			var entities = await _unitOfWork.SummaryOfSalesByYearRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<SummaryOfSalesByYearModel>>(entities);
		}

	}
	public partial class SuppliersService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public SuppliersService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public virtual async Task<IEnumerable<SuppliersModel>> GetAllAsync()
		{
			var entities = await _unitOfWork.SuppliersRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<SuppliersModel>>(entities);
		}

		public virtual async Task<SuppliersModel> GetByIdAsync(int supplierId)
		{
			var entity = await _unitOfWork.SuppliersRepository.GetByIdAsync(supplierId);
			return _mapper.Map<SuppliersModel>(entity);
		}

		public virtual async Task DeleteAsync(int supplierId)
		{
			await _unitOfWork.SuppliersRepository.DeleteAsync(supplierId);
			await _unitOfWork.SaveChangesAsync();
		}

		public virtual async Task<SuppliersModel> AddAsync(SuppliersModel model)
		{
			var entity = _mapper.Map<Suppliers>(model);
			entity = await _unitOfWork.SuppliersRepository.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<SuppliersModel>(entity);
		}

		public virtual async Task<SuppliersModel> UpdateAsync(SuppliersModel model)
		{
			var entity = _mapper.Map<Suppliers>(model);
			entity = await _unitOfWork.SuppliersRepository.UpdateAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<SuppliersModel>(entity);
		}
	}
	public partial class TerritoriesService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public TerritoriesService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public virtual async Task<IEnumerable<TerritoriesModel>> GetAllAsync()
		{
			var entities = await _unitOfWork.TerritoriesRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<TerritoriesModel>>(entities);
		}

		public virtual async Task<TerritoriesModel> GetByIdAsync(string territoryId)
		{
			var entity = await _unitOfWork.TerritoriesRepository.GetByIdAsync(territoryId);
			return _mapper.Map<TerritoriesModel>(entity);
		}

		public virtual async Task DeleteAsync(string territoryId)
		{
			await _unitOfWork.TerritoriesRepository.DeleteAsync(territoryId);
			await _unitOfWork.SaveChangesAsync();
		}

		public virtual async Task<TerritoriesModel> AddAsync(TerritoriesModel model)
		{
			var entity = _mapper.Map<Territories>(model);
			entity = await _unitOfWork.TerritoriesRepository.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<TerritoriesModel>(entity);
		}

		public virtual async Task<TerritoriesModel> UpdateAsync(TerritoriesModel model)
		{
			var entity = _mapper.Map<Territories>(model);
			entity = await _unitOfWork.TerritoriesRepository.UpdateAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<TerritoriesModel>(entity);
		}
	}
}