using AutoMapper;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Domain.Entities;
using MPR.RestApiTemplate.Domain.Interfaces;
using System.Linq.Expressions;

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

		public virtual async Task<IEnumerable<AlphabeticalListOfProductsDto>> GetAllAsync(params Expression<Func<AlphabeticalListOfProducts, object>>[] includes)
		{
			var entities = await _unitOfWork.AlphabeticalListOfProductsRepository.GetAllAsync(includes);
			return _mapper.Map<IEnumerable<AlphabeticalListOfProductsDto>>(entities);
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

		public virtual async Task<IEnumerable<CategoriesDto>> GetAllAsync(params Expression<Func<Categories, object>>[] includes)
		{
			var entities = await _unitOfWork.CategoriesRepository.GetAllAsync(includes);
			return _mapper.Map<IEnumerable<CategoriesDto>>(entities);
		}

		public virtual async Task<CategoriesDto> GetByIdAsync(int categoryId, params Expression<Func<Categories, object>>[] includes)
		{
			var entity = await _unitOfWork.CategoriesRepository.GetByIdAsync(new object[] { categoryId }, includes);
			return _mapper.Map<CategoriesDto>(entity);
		}

		public virtual async Task DeleteAsync(int categoryId)
		{
			await _unitOfWork.CategoriesRepository.DeleteAsync(categoryId);
			await _unitOfWork.SaveChangesAsync();
		}

		public virtual async Task<CategoriesDto> AddAsync(CategoriesCreateDto model)
		{
			var entity = _mapper.Map<Categories>(model);
			entity = await _unitOfWork.CategoriesRepository.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<CategoriesDto>(entity);
		}

		public virtual async Task<CategoriesDto> UpdateAsync(CategoriesUpdateDto model)
		{
			var entity = _mapper.Map<Categories>(model);
			entity = await _unitOfWork.CategoriesRepository.UpdateAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<CategoriesDto>(entity);
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

		public virtual async Task<IEnumerable<CategorySalesFor1997Dto>> GetAllAsync(params Expression<Func<CategorySalesFor1997, object>>[] includes)
		{
			var entities = await _unitOfWork.CategorySalesFor1997Repository.GetAllAsync(includes);
			return _mapper.Map<IEnumerable<CategorySalesFor1997Dto>>(entities);
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

		public virtual async Task<IEnumerable<CurrentProductListDto>> GetAllAsync(params Expression<Func<CurrentProductList, object>>[] includes)
		{
			var entities = await _unitOfWork.CurrentProductListRepository.GetAllAsync(includes);
			return _mapper.Map<IEnumerable<CurrentProductListDto>>(entities);
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

		public virtual async Task<IEnumerable<CustomerAndSuppliersByCityDto>> GetAllAsync(params Expression<Func<CustomerAndSuppliersByCity, object>>[] includes)
		{
			var entities = await _unitOfWork.CustomerAndSuppliersByCityRepository.GetAllAsync(includes);
			return _mapper.Map<IEnumerable<CustomerAndSuppliersByCityDto>>(entities);
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

		public virtual async Task<IEnumerable<CustomerDemographicsDto>> GetAllAsync(params Expression<Func<CustomerDemographics, object>>[] includes)
		{
			var entities = await _unitOfWork.CustomerDemographicsRepository.GetAllAsync(includes);
			return _mapper.Map<IEnumerable<CustomerDemographicsDto>>(entities);
		}

		public virtual async Task<CustomerDemographicsDto> GetByIdAsync(string customerTypeId, params Expression<Func<CustomerDemographics, object>>[] includes)
		{
			var entity = await _unitOfWork.CustomerDemographicsRepository.GetByIdAsync(new object[] { customerTypeId }, includes);
			return _mapper.Map<CustomerDemographicsDto>(entity);
		}

		public virtual async Task DeleteAsync(string customerTypeId)
		{
			await _unitOfWork.CustomerDemographicsRepository.DeleteAsync(customerTypeId);
			await _unitOfWork.SaveChangesAsync();
		}

		public virtual async Task<CustomerDemographicsDto> AddAsync(CustomerDemographicsCreateDto model)
		{
			var entity = _mapper.Map<CustomerDemographics>(model);
			entity = await _unitOfWork.CustomerDemographicsRepository.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<CustomerDemographicsDto>(entity);
		}

		public virtual async Task<CustomerDemographicsDto> UpdateAsync(CustomerDemographicsUpdateDto model)
		{
			var entity = _mapper.Map<CustomerDemographics>(model);
			entity = await _unitOfWork.CustomerDemographicsRepository.UpdateAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<CustomerDemographicsDto>(entity);
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

		public virtual async Task<IEnumerable<CustomersDto>> GetAllAsync(params Expression<Func<Customers, object>>[] includes)
		{
			var entities = await _unitOfWork.CustomersRepository.GetAllAsync(includes);
			return _mapper.Map<IEnumerable<CustomersDto>>(entities);
		}

		public virtual async Task<CustomersDto> GetByIdAsync(string customerId, params Expression<Func<Customers, object>>[] includes)
		{
			var entity = await _unitOfWork.CustomersRepository.GetByIdAsync(new object[] { customerId }, includes);
			return _mapper.Map<CustomersDto>(entity);
		}

		public virtual async Task DeleteAsync(string customerId)
		{
			await _unitOfWork.CustomersRepository.DeleteAsync(customerId);
			await _unitOfWork.SaveChangesAsync();
		}

		public virtual async Task<CustomersDto> AddAsync(CustomersCreateDto model)
		{
			var entity = _mapper.Map<Customers>(model);
			entity = await _unitOfWork.CustomersRepository.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<CustomersDto>(entity);
		}

		public virtual async Task<CustomersDto> UpdateAsync(CustomersUpdateDto model)
		{
			var entity = _mapper.Map<Customers>(model);
			entity = await _unitOfWork.CustomersRepository.UpdateAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<CustomersDto>(entity);
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

		public virtual async Task<IEnumerable<EmployeesDto>> GetAllAsync(params Expression<Func<Employees, object>>[] includes)
		{
			var entities = await _unitOfWork.EmployeesRepository.GetAllAsync(includes);
			return _mapper.Map<IEnumerable<EmployeesDto>>(entities);
		}

		public virtual async Task<EmployeesDto> GetByIdAsync(int employeeId, params Expression<Func<Employees, object>>[] includes)
		{
			var entity = await _unitOfWork.EmployeesRepository.GetByIdAsync(new object[] { employeeId }, includes);
			return _mapper.Map<EmployeesDto>(entity);
		}

		public virtual async Task DeleteAsync(int employeeId)
		{
			await _unitOfWork.EmployeesRepository.DeleteAsync(employeeId);
			await _unitOfWork.SaveChangesAsync();
		}

		public virtual async Task<EmployeesDto> AddAsync(EmployeesCreateDto model)
		{
			var entity = _mapper.Map<Employees>(model);
			entity = await _unitOfWork.EmployeesRepository.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<EmployeesDto>(entity);
		}

		public virtual async Task<EmployeesDto> UpdateAsync(EmployeesUpdateDto model)
		{
			var entity = _mapper.Map<Employees>(model);
			entity = await _unitOfWork.EmployeesRepository.UpdateAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<EmployeesDto>(entity);
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

		public virtual async Task<IEnumerable<InvoicesDto>> GetAllAsync(params Expression<Func<Invoices, object>>[] includes)
		{
			var entities = await _unitOfWork.InvoicesRepository.GetAllAsync(includes);
			return _mapper.Map<IEnumerable<InvoicesDto>>(entities);
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

		public virtual async Task<IEnumerable<OrderDetailsDto>> GetAllAsync(params Expression<Func<OrderDetails, object>>[] includes)
		{
			var entities = await _unitOfWork.OrderDetailsRepository.GetAllAsync(includes);
			return _mapper.Map<IEnumerable<OrderDetailsDto>>(entities);
		}

		public virtual async Task<OrderDetailsDto> GetByIdAsync(int orderId, int productId, params Expression<Func<OrderDetails, object>>[] includes)
		{
			var entity = await _unitOfWork.OrderDetailsRepository.GetByIdAsync(new object[] { orderId, productId }, includes);
			return _mapper.Map<OrderDetailsDto>(entity);
		}

		public virtual async Task DeleteAsync(int orderId, int productId)
		{
			await _unitOfWork.OrderDetailsRepository.DeleteAsync(orderId, productId);
			await _unitOfWork.SaveChangesAsync();
		}

		public virtual async Task<OrderDetailsDto> AddAsync(OrderDetailsCreateDto model)
		{
			var entity = _mapper.Map<OrderDetails>(model);
			entity = await _unitOfWork.OrderDetailsRepository.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<OrderDetailsDto>(entity);
		}

		public virtual async Task<OrderDetailsDto> UpdateAsync(OrderDetailsUpdateDto model)
		{
			var entity = _mapper.Map<OrderDetails>(model);
			entity = await _unitOfWork.OrderDetailsRepository.UpdateAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<OrderDetailsDto>(entity);
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

		public virtual async Task<IEnumerable<OrderDetailsExtendedDto>> GetAllAsync(params Expression<Func<OrderDetailsExtended, object>>[] includes)
		{
			var entities = await _unitOfWork.OrderDetailsExtendedRepository.GetAllAsync(includes);
			return _mapper.Map<IEnumerable<OrderDetailsExtendedDto>>(entities);
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

		public virtual async Task<IEnumerable<OrdersDto>> GetAllAsync(params Expression<Func<Orders, object>>[] includes)
		{
			var entities = await _unitOfWork.OrdersRepository.GetAllAsync(includes);
			return _mapper.Map<IEnumerable<OrdersDto>>(entities);
		}

		public virtual async Task<OrdersDto> GetByIdAsync(int orderId, params Expression<Func<Orders, object>>[] includes)
		{
			var entity = await _unitOfWork.OrdersRepository.GetByIdAsync(new object[] { orderId }, includes);
			return _mapper.Map<OrdersDto>(entity);
		}

		public virtual async Task DeleteAsync(int orderId)
		{
			await _unitOfWork.OrdersRepository.DeleteAsync(orderId);
			await _unitOfWork.SaveChangesAsync();
		}

		public virtual async Task<OrdersDto> AddAsync(OrdersCreateDto model)
		{
			var entity = _mapper.Map<Orders>(model);
			entity = await _unitOfWork.OrdersRepository.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<OrdersDto>(entity);
		}

		public virtual async Task<OrdersDto> UpdateAsync(OrdersUpdateDto model)
		{
			var entity = _mapper.Map<Orders>(model);
			entity = await _unitOfWork.OrdersRepository.UpdateAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<OrdersDto>(entity);
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

		public virtual async Task<IEnumerable<OrdersQryDto>> GetAllAsync(params Expression<Func<OrdersQry, object>>[] includes)
		{
			var entities = await _unitOfWork.OrdersQryRepository.GetAllAsync(includes);
			return _mapper.Map<IEnumerable<OrdersQryDto>>(entities);
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

		public virtual async Task<IEnumerable<OrderSubtotalsDto>> GetAllAsync(params Expression<Func<OrderSubtotals, object>>[] includes)
		{
			var entities = await _unitOfWork.OrderSubtotalsRepository.GetAllAsync(includes);
			return _mapper.Map<IEnumerable<OrderSubtotalsDto>>(entities);
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

		public virtual async Task<IEnumerable<ProductsDto>> GetAllAsync(params Expression<Func<Products, object>>[] includes)
		{
			var entities = await _unitOfWork.ProductsRepository.GetAllAsync(includes);
			return _mapper.Map<IEnumerable<ProductsDto>>(entities);
		}

		public virtual async Task<ProductsDto> GetByIdAsync(int productId, params Expression<Func<Products, object>>[] includes)
		{
			var entity = await _unitOfWork.ProductsRepository.GetByIdAsync(new object[] { productId }, includes);
			return _mapper.Map<ProductsDto>(entity);
		}

		public virtual async Task DeleteAsync(int productId)
		{
			await _unitOfWork.ProductsRepository.DeleteAsync(productId);
			await _unitOfWork.SaveChangesAsync();
		}

		public virtual async Task<ProductsDto> AddAsync(ProductsCreateDto model)
		{
			var entity = _mapper.Map<Products>(model);
			entity = await _unitOfWork.ProductsRepository.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<ProductsDto>(entity);
		}

		public virtual async Task<ProductsDto> UpdateAsync(ProductsUpdateDto model)
		{
			var entity = _mapper.Map<Products>(model);
			entity = await _unitOfWork.ProductsRepository.UpdateAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<ProductsDto>(entity);
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

		public virtual async Task<IEnumerable<ProductsAboveAveragePriceDto>> GetAllAsync(params Expression<Func<ProductsAboveAveragePrice, object>>[] includes)
		{
			var entities = await _unitOfWork.ProductsAboveAveragePriceRepository.GetAllAsync(includes);
			return _mapper.Map<IEnumerable<ProductsAboveAveragePriceDto>>(entities);
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

		public virtual async Task<IEnumerable<ProductSalesFor1997Dto>> GetAllAsync(params Expression<Func<ProductSalesFor1997, object>>[] includes)
		{
			var entities = await _unitOfWork.ProductSalesFor1997Repository.GetAllAsync(includes);
			return _mapper.Map<IEnumerable<ProductSalesFor1997Dto>>(entities);
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

		public virtual async Task<IEnumerable<ProductsByCategoryDto>> GetAllAsync(params Expression<Func<ProductsByCategory, object>>[] includes)
		{
			var entities = await _unitOfWork.ProductsByCategoryRepository.GetAllAsync(includes);
			return _mapper.Map<IEnumerable<ProductsByCategoryDto>>(entities);
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

		public virtual async Task<IEnumerable<QuarterlyOrdersDto>> GetAllAsync(params Expression<Func<QuarterlyOrders, object>>[] includes)
		{
			var entities = await _unitOfWork.QuarterlyOrdersRepository.GetAllAsync(includes);
			return _mapper.Map<IEnumerable<QuarterlyOrdersDto>>(entities);
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

		public virtual async Task<IEnumerable<RegionDto>> GetAllAsync(params Expression<Func<Region, object>>[] includes)
		{
			var entities = await _unitOfWork.RegionRepository.GetAllAsync(includes);
			return _mapper.Map<IEnumerable<RegionDto>>(entities);
		}

		public virtual async Task<RegionDto> GetByIdAsync(int regionId, params Expression<Func<Region, object>>[] includes)
		{
			var entity = await _unitOfWork.RegionRepository.GetByIdAsync(new object[] { regionId }, includes);
			return _mapper.Map<RegionDto>(entity);
		}

		public virtual async Task DeleteAsync(int regionId)
		{
			await _unitOfWork.RegionRepository.DeleteAsync(regionId);
			await _unitOfWork.SaveChangesAsync();
		}

		public virtual async Task<RegionDto> AddAsync(RegionCreateDto model)
		{
			var entity = _mapper.Map<Region>(model);
			entity = await _unitOfWork.RegionRepository.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<RegionDto>(entity);
		}

		public virtual async Task<RegionDto> UpdateAsync(RegionUpdateDto model)
		{
			var entity = _mapper.Map<Region>(model);
			entity = await _unitOfWork.RegionRepository.UpdateAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<RegionDto>(entity);
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

		public virtual async Task<IEnumerable<SalesByCategoryDto>> GetAllAsync(params Expression<Func<SalesByCategory, object>>[] includes)
		{
			var entities = await _unitOfWork.SalesByCategoryRepository.GetAllAsync(includes);
			return _mapper.Map<IEnumerable<SalesByCategoryDto>>(entities);
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

		public virtual async Task<IEnumerable<SalesTotalsByAmountDto>> GetAllAsync(params Expression<Func<SalesTotalsByAmount, object>>[] includes)
		{
			var entities = await _unitOfWork.SalesTotalsByAmountRepository.GetAllAsync(includes);
			return _mapper.Map<IEnumerable<SalesTotalsByAmountDto>>(entities);
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

		public virtual async Task<IEnumerable<ShippersDto>> GetAllAsync(params Expression<Func<Shippers, object>>[] includes)
		{
			var entities = await _unitOfWork.ShippersRepository.GetAllAsync(includes);
			return _mapper.Map<IEnumerable<ShippersDto>>(entities);
		}

		public virtual async Task<ShippersDto> GetByIdAsync(int shipperId, params Expression<Func<Shippers, object>>[] includes)
		{
			var entity = await _unitOfWork.ShippersRepository.GetByIdAsync(new object[] { shipperId }, includes);
			return _mapper.Map<ShippersDto>(entity);
		}

		public virtual async Task DeleteAsync(int shipperId)
		{
			await _unitOfWork.ShippersRepository.DeleteAsync(shipperId);
			await _unitOfWork.SaveChangesAsync();
		}

		public virtual async Task<ShippersDto> AddAsync(ShippersCreateDto model)
		{
			var entity = _mapper.Map<Shippers>(model);
			entity = await _unitOfWork.ShippersRepository.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<ShippersDto>(entity);
		}

		public virtual async Task<ShippersDto> UpdateAsync(ShippersUpdateDto model)
		{
			var entity = _mapper.Map<Shippers>(model);
			entity = await _unitOfWork.ShippersRepository.UpdateAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<ShippersDto>(entity);
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

		public virtual async Task<IEnumerable<SummaryOfSalesByQuarterDto>> GetAllAsync(params Expression<Func<SummaryOfSalesByQuarter, object>>[] includes)
		{
			var entities = await _unitOfWork.SummaryOfSalesByQuarterRepository.GetAllAsync(includes);
			return _mapper.Map<IEnumerable<SummaryOfSalesByQuarterDto>>(entities);
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

		public virtual async Task<IEnumerable<SummaryOfSalesByYearDto>> GetAllAsync(params Expression<Func<SummaryOfSalesByYear, object>>[] includes)
		{
			var entities = await _unitOfWork.SummaryOfSalesByYearRepository.GetAllAsync(includes);
			return _mapper.Map<IEnumerable<SummaryOfSalesByYearDto>>(entities);
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

		public virtual async Task<IEnumerable<SuppliersDto>> GetAllAsync(params Expression<Func<Suppliers, object>>[] includes)
		{
			var entities = await _unitOfWork.SuppliersRepository.GetAllAsync(includes);
			return _mapper.Map<IEnumerable<SuppliersDto>>(entities);
		}

		public virtual async Task<SuppliersDto> GetByIdAsync(int supplierId, params Expression<Func<Suppliers, object>>[] includes)
		{
			var entity = await _unitOfWork.SuppliersRepository.GetByIdAsync(new object[] { supplierId }, includes);
			return _mapper.Map<SuppliersDto>(entity);
		}

		public virtual async Task DeleteAsync(int supplierId)
		{
			await _unitOfWork.SuppliersRepository.DeleteAsync(supplierId);
			await _unitOfWork.SaveChangesAsync();
		}

		public virtual async Task<SuppliersDto> AddAsync(SuppliersCreateDto model)
		{
			var entity = _mapper.Map<Suppliers>(model);
			entity = await _unitOfWork.SuppliersRepository.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<SuppliersDto>(entity);
		}

		public virtual async Task<SuppliersDto> UpdateAsync(SuppliersUpdateDto model)
		{
			var entity = _mapper.Map<Suppliers>(model);
			entity = await _unitOfWork.SuppliersRepository.UpdateAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<SuppliersDto>(entity);
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

		public virtual async Task<IEnumerable<TerritoriesDto>> GetAllAsync(params Expression<Func<Territories, object>>[] includes)
		{
			var entities = await _unitOfWork.TerritoriesRepository.GetAllAsync(includes);
			return _mapper.Map<IEnumerable<TerritoriesDto>>(entities);
		}

		public virtual async Task<TerritoriesDto> GetByIdAsync(string territoryId, params Expression<Func<Territories, object>>[] includes)
		{
			var entity = await _unitOfWork.TerritoriesRepository.GetByIdAsync(new object[] { territoryId }, includes);
			return _mapper.Map<TerritoriesDto>(entity);
		}

		public virtual async Task DeleteAsync(string territoryId)
		{
			await _unitOfWork.TerritoriesRepository.DeleteAsync(territoryId);
			await _unitOfWork.SaveChangesAsync();
		}

		public virtual async Task<TerritoriesDto> AddAsync(TerritoriesCreateDto model)
		{
			var entity = _mapper.Map<Territories>(model);
			entity = await _unitOfWork.TerritoriesRepository.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<TerritoriesDto>(entity);
		}

		public virtual async Task<TerritoriesDto> UpdateAsync(TerritoriesUpdateDto model)
		{
			var entity = _mapper.Map<Territories>(model);
			entity = await _unitOfWork.TerritoriesRepository.UpdateAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<TerritoriesDto>(entity);
		}
	}
}