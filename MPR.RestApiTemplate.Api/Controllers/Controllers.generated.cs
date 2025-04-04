using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Application.Services;

namespace MPR.RestApiTemplate.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class AlphabeticalListOfProductsController : ControllerBase
    {
        private readonly AlphabeticalListOfProductsService _service;

        public AlphabeticalListOfProductsController(AlphabeticalListOfProductsService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<AlphabeticalListOfProductsDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
    }
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class CategoriesController : ControllerBase
    {
        private readonly CategoriesService _service;

        public CategoriesController(CategoriesService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<CategoriesDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{categoryId}")]
        public virtual async Task<ActionResult<CategoriesDto>> GetById(int categoryId)
        {
            var result = await _service.GetByIdAsync(categoryId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{categoryId}")]
        public virtual async Task<ActionResult> DeleteAsync(int categoryId)
        {
            await _service.DeleteAsync(categoryId);
            return NoContent();
        }

        [HttpPut("{categoryId}")]
        public virtual async Task<ActionResult<CategoriesDto>> UpdateAsync(int categoryId, [FromBody] CategoriesUpdateDto model)
        {
            if (model.CategoryId != categoryId)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<CategoriesDto>> AddAsync([FromBody] CategoriesCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { CategoryId = result.CategoryId }, result);
        }
    }
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class CategorySalesFor1997Controller : ControllerBase
    {
        private readonly CategorySalesFor1997Service _service;

        public CategorySalesFor1997Controller(CategorySalesFor1997Service service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<CategorySalesFor1997Dto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
    }
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class CurrentProductListController : ControllerBase
    {
        private readonly CurrentProductListService _service;

        public CurrentProductListController(CurrentProductListService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<CurrentProductListDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
    }
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class CustomerAndSuppliersByCityController : ControllerBase
    {
        private readonly CustomerAndSuppliersByCityService _service;

        public CustomerAndSuppliersByCityController(CustomerAndSuppliersByCityService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<CustomerAndSuppliersByCityDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
    }
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class CustomerDemographicsController : ControllerBase
    {
        private readonly CustomerDemographicsService _service;

        public CustomerDemographicsController(CustomerDemographicsService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<CustomerDemographicsDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{customerTypeId}")]
        public virtual async Task<ActionResult<CustomerDemographicsDto>> GetById(string customerTypeId)
        {
            var result = await _service.GetByIdAsync(customerTypeId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{customerTypeId}")]
        public virtual async Task<ActionResult> DeleteAsync(string customerTypeId)
        {
            await _service.DeleteAsync(customerTypeId);
            return NoContent();
        }

        [HttpPut("{customerTypeId}")]
        public virtual async Task<ActionResult<CustomerDemographicsDto>> UpdateAsync(string customerTypeId, [FromBody] CustomerDemographicsUpdateDto model)
        {
            if (model.CustomerTypeId != customerTypeId)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<CustomerDemographicsDto>> AddAsync([FromBody] CustomerDemographicsCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { CustomerTypeId = result.CustomerTypeId }, result);
        }
    }
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class CustomersController : ControllerBase
    {
        private readonly CustomersService _service;

        public CustomersController(CustomersService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<CustomersDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{customerId}")]
        public virtual async Task<ActionResult<CustomersDto>> GetById(string customerId)
        {
            var result = await _service.GetByIdAsync(customerId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{customerId}")]
        public virtual async Task<ActionResult> DeleteAsync(string customerId)
        {
            await _service.DeleteAsync(customerId);
            return NoContent();
        }

        [HttpPut("{customerId}")]
        public virtual async Task<ActionResult<CustomersDto>> UpdateAsync(string customerId, [FromBody] CustomersUpdateDto model)
        {
            if (model.CustomerId != customerId)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<CustomersDto>> AddAsync([FromBody] CustomersCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { CustomerId = result.CustomerId }, result);
        }
    }
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class EmployeesController : ControllerBase
    {
        private readonly EmployeesService _service;

        public EmployeesController(EmployeesService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<EmployeesDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{employeeId}")]
        public virtual async Task<ActionResult<EmployeesDto>> GetById(int employeeId)
        {
            var result = await _service.GetByIdAsync(employeeId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{employeeId}")]
        public virtual async Task<ActionResult> DeleteAsync(int employeeId)
        {
            await _service.DeleteAsync(employeeId);
            return NoContent();
        }

        [HttpPut("{employeeId}")]
        public virtual async Task<ActionResult<EmployeesDto>> UpdateAsync(int employeeId, [FromBody] EmployeesUpdateDto model)
        {
            if (model.EmployeeId != employeeId)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<EmployeesDto>> AddAsync([FromBody] EmployeesCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { EmployeeId = result.EmployeeId }, result);
        }
    }
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class InvoicesController : ControllerBase
    {
        private readonly InvoicesService _service;

        public InvoicesController(InvoicesService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<InvoicesDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
    }
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class OrderDetailsController : ControllerBase
    {
        private readonly OrderDetailsService _service;

        public OrderDetailsController(OrderDetailsService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<OrderDetailsDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{orderId}/{productId}")]
        public virtual async Task<ActionResult<OrderDetailsDto>> GetById(int orderId, int productId)
        {
            var result = await _service.GetByIdAsync(orderId, productId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{orderId}/{productId}")]
        public virtual async Task<ActionResult> DeleteAsync(int orderId, int productId)
        {
            await _service.DeleteAsync(orderId, productId);
            return NoContent();
        }

        [HttpPut("{orderId}/{productId}")]
        public virtual async Task<ActionResult<OrderDetailsDto>> UpdateAsync(int orderId, int productId, [FromBody] OrderDetailsUpdateDto model)
        {
            if (model.OrderId != orderId)
                return BadRequest("Key mismatch between route and payload");
            if (model.ProductId != productId)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<OrderDetailsDto>> AddAsync([FromBody] OrderDetailsCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { OrderId = result.OrderId, ProductId = result.ProductId }, result);
        }
    }
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class OrderDetailsExtendedController : ControllerBase
    {
        private readonly OrderDetailsExtendedService _service;

        public OrderDetailsExtendedController(OrderDetailsExtendedService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<OrderDetailsExtendedDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
    }
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class OrdersController : ControllerBase
    {
        private readonly OrdersService _service;

        public OrdersController(OrdersService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<OrdersDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{orderId}")]
        public virtual async Task<ActionResult<OrdersDto>> GetById(int orderId)
        {
            var result = await _service.GetByIdAsync(orderId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{orderId}")]
        public virtual async Task<ActionResult> DeleteAsync(int orderId)
        {
            await _service.DeleteAsync(orderId);
            return NoContent();
        }

        [HttpPut("{orderId}")]
        public virtual async Task<ActionResult<OrdersDto>> UpdateAsync(int orderId, [FromBody] OrdersUpdateDto model)
        {
            if (model.OrderId != orderId)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<OrdersDto>> AddAsync([FromBody] OrdersCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { OrderId = result.OrderId }, result);
        }
    }
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class OrdersQryController : ControllerBase
    {
        private readonly OrdersQryService _service;

        public OrdersQryController(OrdersQryService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<OrdersQryDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
    }
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class OrderSubtotalsController : ControllerBase
    {
        private readonly OrderSubtotalsService _service;

        public OrderSubtotalsController(OrderSubtotalsService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<OrderSubtotalsDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
    }
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class ProductsController : ControllerBase
    {
        private readonly ProductsService _service;

        public ProductsController(ProductsService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<ProductsDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{productId}")]
        public virtual async Task<ActionResult<ProductsDto>> GetById(int productId)
        {
            var result = await _service.GetByIdAsync(productId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{productId}")]
        public virtual async Task<ActionResult> DeleteAsync(int productId)
        {
            await _service.DeleteAsync(productId);
            return NoContent();
        }

        [HttpPut("{productId}")]
        public virtual async Task<ActionResult<ProductsDto>> UpdateAsync(int productId, [FromBody] ProductsUpdateDto model)
        {
            if (model.ProductId != productId)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<ProductsDto>> AddAsync([FromBody] ProductsCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { ProductId = result.ProductId }, result);
        }
    }
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class ProductsAboveAveragePriceController : ControllerBase
    {
        private readonly ProductsAboveAveragePriceService _service;

        public ProductsAboveAveragePriceController(ProductsAboveAveragePriceService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<ProductsAboveAveragePriceDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
    }
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class ProductSalesFor1997Controller : ControllerBase
    {
        private readonly ProductSalesFor1997Service _service;

        public ProductSalesFor1997Controller(ProductSalesFor1997Service service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<ProductSalesFor1997Dto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
    }
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class ProductsByCategoryController : ControllerBase
    {
        private readonly ProductsByCategoryService _service;

        public ProductsByCategoryController(ProductsByCategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<ProductsByCategoryDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
    }
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class QuarterlyOrdersController : ControllerBase
    {
        private readonly QuarterlyOrdersService _service;

        public QuarterlyOrdersController(QuarterlyOrdersService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<QuarterlyOrdersDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
    }
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class RegionController : ControllerBase
    {
        private readonly RegionService _service;

        public RegionController(RegionService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<RegionDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{regionId}")]
        public virtual async Task<ActionResult<RegionDto>> GetById(int regionId)
        {
            var result = await _service.GetByIdAsync(regionId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{regionId}")]
        public virtual async Task<ActionResult> DeleteAsync(int regionId)
        {
            await _service.DeleteAsync(regionId);
            return NoContent();
        }

        [HttpPut("{regionId}")]
        public virtual async Task<ActionResult<RegionDto>> UpdateAsync(int regionId, [FromBody] RegionUpdateDto model)
        {
            if (model.RegionId != regionId)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<RegionDto>> AddAsync([FromBody] RegionCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { RegionId = result.RegionId }, result);
        }
    }
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class SalesByCategoryController : ControllerBase
    {
        private readonly SalesByCategoryService _service;

        public SalesByCategoryController(SalesByCategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<SalesByCategoryDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
    }
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class SalesTotalsByAmountController : ControllerBase
    {
        private readonly SalesTotalsByAmountService _service;

        public SalesTotalsByAmountController(SalesTotalsByAmountService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<SalesTotalsByAmountDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
    }
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class ShippersController : ControllerBase
    {
        private readonly ShippersService _service;

        public ShippersController(ShippersService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<ShippersDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{shipperId}")]
        public virtual async Task<ActionResult<ShippersDto>> GetById(int shipperId)
        {
            var result = await _service.GetByIdAsync(shipperId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{shipperId}")]
        public virtual async Task<ActionResult> DeleteAsync(int shipperId)
        {
            await _service.DeleteAsync(shipperId);
            return NoContent();
        }

        [HttpPut("{shipperId}")]
        public virtual async Task<ActionResult<ShippersDto>> UpdateAsync(int shipperId, [FromBody] ShippersUpdateDto model)
        {
            if (model.ShipperId != shipperId)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<ShippersDto>> AddAsync([FromBody] ShippersCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { ShipperId = result.ShipperId }, result);
        }
    }
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class SummaryOfSalesByQuarterController : ControllerBase
    {
        private readonly SummaryOfSalesByQuarterService _service;

        public SummaryOfSalesByQuarterController(SummaryOfSalesByQuarterService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<SummaryOfSalesByQuarterDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
    }
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class SummaryOfSalesByYearController : ControllerBase
    {
        private readonly SummaryOfSalesByYearService _service;

        public SummaryOfSalesByYearController(SummaryOfSalesByYearService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<SummaryOfSalesByYearDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
    }
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class SuppliersController : ControllerBase
    {
        private readonly SuppliersService _service;

        public SuppliersController(SuppliersService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<SuppliersDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{supplierId}")]
        public virtual async Task<ActionResult<SuppliersDto>> GetById(int supplierId)
        {
            var result = await _service.GetByIdAsync(supplierId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{supplierId}")]
        public virtual async Task<ActionResult> DeleteAsync(int supplierId)
        {
            await _service.DeleteAsync(supplierId);
            return NoContent();
        }

        [HttpPut("{supplierId}")]
        public virtual async Task<ActionResult<SuppliersDto>> UpdateAsync(int supplierId, [FromBody] SuppliersUpdateDto model)
        {
            if (model.SupplierId != supplierId)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<SuppliersDto>> AddAsync([FromBody] SuppliersCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { SupplierId = result.SupplierId }, result);
        }
    }
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class TerritoriesController : ControllerBase
    {
        private readonly TerritoriesService _service;

        public TerritoriesController(TerritoriesService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TerritoriesDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{territoryId}")]
        public virtual async Task<ActionResult<TerritoriesDto>> GetById(string territoryId)
        {
            var result = await _service.GetByIdAsync(territoryId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{territoryId}")]
        public virtual async Task<ActionResult> DeleteAsync(string territoryId)
        {
            await _service.DeleteAsync(territoryId);
            return NoContent();
        }

        [HttpPut("{territoryId}")]
        public virtual async Task<ActionResult<TerritoriesDto>> UpdateAsync(string territoryId, [FromBody] TerritoriesUpdateDto model)
        {
            if (model.TerritoryId != territoryId)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<TerritoriesDto>> AddAsync([FromBody] TerritoriesCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { TerritoryId = result.TerritoryId }, result);
        }
    }
}