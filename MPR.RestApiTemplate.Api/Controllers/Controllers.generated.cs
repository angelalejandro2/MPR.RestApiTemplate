using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.Models;
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
        public virtual async Task<ActionResult<IEnumerable<AlphabeticalListOfProductsModel>>> GetAllAsync()
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
        public virtual async Task<ActionResult<IEnumerable<CategoriesModel>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{categoryId}")]
        public virtual async Task<ActionResult<CategoriesModel>> GetById(int categoryId)
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
        public virtual async Task<ActionResult<CategoriesModel>> UpdateAsync(int categoryId, [FromBody] CategoriesModel model)
        {
            if (model.CategoryId != categoryId)
                return BadRequest("Key mismatch between route and payload");

            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<CategoriesModel>> AddASync([FromBody] CategoriesModel model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { result.CategoryId }, result);
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
        public virtual async Task<ActionResult<IEnumerable<CategorySalesFor1997Model>>> GetAllAsync()
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
        public virtual async Task<ActionResult<IEnumerable<CurrentProductListModel>>> GetAllAsync()
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
        public virtual async Task<ActionResult<IEnumerable<CustomerAndSuppliersByCityModel>>> GetAllAsync()
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
        public virtual async Task<ActionResult<IEnumerable<CustomerDemographicsModel>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{customerTypeId}")]
        public virtual async Task<ActionResult<CustomerDemographicsModel>> GetById(string customerTypeId)
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
        public virtual async Task<ActionResult<CustomerDemographicsModel>> UpdateAsync(string customerTypeId, [FromBody] CustomerDemographicsModel model)
        {
            if (model.CustomerTypeId != customerTypeId)
                return BadRequest("Key mismatch between route and payload");

            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<CustomerDemographicsModel>> AddASync([FromBody] CustomerDemographicsModel model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { result.CustomerTypeId }, result);
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
        public virtual async Task<ActionResult<IEnumerable<CustomersModel>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{customerId}")]
        public virtual async Task<ActionResult<CustomersModel>> GetById(string customerId)
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
        public virtual async Task<ActionResult<CustomersModel>> UpdateAsync(string customerId, [FromBody] CustomersModel model)
        {
            if (model.CustomerId != customerId)
                return BadRequest("Key mismatch between route and payload");

            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<CustomersModel>> AddASync([FromBody] CustomersModel model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { result.CustomerId }, result);
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
        public virtual async Task<ActionResult<IEnumerable<EmployeesModel>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{employeeId}")]
        public virtual async Task<ActionResult<EmployeesModel>> GetById(int employeeId)
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
        public virtual async Task<ActionResult<EmployeesModel>> UpdateAsync(int employeeId, [FromBody] EmployeesModel model)
        {
            if (model.EmployeeId != employeeId)
                return BadRequest("Key mismatch between route and payload");

            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<EmployeesModel>> AddASync([FromBody] EmployeesModel model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { result.EmployeeId }, result);
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
        public virtual async Task<ActionResult<IEnumerable<InvoicesModel>>> GetAllAsync()
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
        public virtual async Task<ActionResult<IEnumerable<OrderDetailsModel>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{orderId}/{productId}")]
        public virtual async Task<ActionResult<OrderDetailsModel>> GetById(int orderId, int productId)
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
        public virtual async Task<ActionResult<OrderDetailsModel>> UpdateAsync(int orderId, int productId, [FromBody] OrderDetailsModel model)
        {
            if (model.OrderId != orderId)
                return BadRequest("Key mismatch between route and payload");
            if (model.ProductId != productId)
                return BadRequest("Key mismatch between route and payload");

            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<OrderDetailsModel>> AddASync([FromBody] OrderDetailsModel model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { result.OrderId, result.ProductId }, result);
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
        public virtual async Task<ActionResult<IEnumerable<OrderDetailsExtendedModel>>> GetAllAsync()
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
        public virtual async Task<ActionResult<IEnumerable<OrdersModel>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{orderId}")]
        public virtual async Task<ActionResult<OrdersModel>> GetById(int orderId)
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
        public virtual async Task<ActionResult<OrdersModel>> UpdateAsync(int orderId, [FromBody] OrdersModel model)
        {
            if (model.OrderId != orderId)
                return BadRequest("Key mismatch between route and payload");

            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<OrdersModel>> AddASync([FromBody] OrdersModel model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { result.OrderId }, result);
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
        public virtual async Task<ActionResult<IEnumerable<OrdersQryModel>>> GetAllAsync()
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
        public virtual async Task<ActionResult<IEnumerable<OrderSubtotalsModel>>> GetAllAsync()
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
        public virtual async Task<ActionResult<IEnumerable<ProductsModel>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{productId}")]
        public virtual async Task<ActionResult<ProductsModel>> GetById(int productId)
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
        public virtual async Task<ActionResult<ProductsModel>> UpdateAsync(int productId, [FromBody] ProductsModel model)
        {
            if (model.ProductId != productId)
                return BadRequest("Key mismatch between route and payload");

            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<ProductsModel>> AddASync([FromBody] ProductsModel model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { result.ProductId }, result);
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
        public virtual async Task<ActionResult<IEnumerable<ProductsAboveAveragePriceModel>>> GetAllAsync()
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
        public virtual async Task<ActionResult<IEnumerable<ProductSalesFor1997Model>>> GetAllAsync()
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
        public virtual async Task<ActionResult<IEnumerable<ProductsByCategoryModel>>> GetAllAsync()
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
        public virtual async Task<ActionResult<IEnumerable<QuarterlyOrdersModel>>> GetAllAsync()
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
        public virtual async Task<ActionResult<IEnumerable<RegionModel>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{regionId}")]
        public virtual async Task<ActionResult<RegionModel>> GetById(int regionId)
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
        public virtual async Task<ActionResult<RegionModel>> UpdateAsync(int regionId, [FromBody] RegionModel model)
        {
            if (model.RegionId != regionId)
                return BadRequest("Key mismatch between route and payload");

            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<RegionModel>> AddASync([FromBody] RegionModel model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { result.RegionId }, result);
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
        public virtual async Task<ActionResult<IEnumerable<SalesByCategoryModel>>> GetAllAsync()
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
        public virtual async Task<ActionResult<IEnumerable<SalesTotalsByAmountModel>>> GetAllAsync()
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
        public virtual async Task<ActionResult<IEnumerable<ShippersModel>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{shipperId}")]
        public virtual async Task<ActionResult<ShippersModel>> GetById(int shipperId)
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
        public virtual async Task<ActionResult<ShippersModel>> UpdateAsync(int shipperId, [FromBody] ShippersModel model)
        {
            if (model.ShipperId != shipperId)
                return BadRequest("Key mismatch between route and payload");

            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<ShippersModel>> AddASync([FromBody] ShippersModel model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { result.ShipperId }, result);
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
        public virtual async Task<ActionResult<IEnumerable<SummaryOfSalesByQuarterModel>>> GetAllAsync()
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
        public virtual async Task<ActionResult<IEnumerable<SummaryOfSalesByYearModel>>> GetAllAsync()
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
        public virtual async Task<ActionResult<IEnumerable<SuppliersModel>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{supplierId}")]
        public virtual async Task<ActionResult<SuppliersModel>> GetById(int supplierId)
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
        public virtual async Task<ActionResult<SuppliersModel>> UpdateAsync(int supplierId, [FromBody] SuppliersModel model)
        {
            if (model.SupplierId != supplierId)
                return BadRequest("Key mismatch between route and payload");

            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<SuppliersModel>> AddASync([FromBody] SuppliersModel model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { result.SupplierId }, result);
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
        public virtual async Task<ActionResult<IEnumerable<TerritoriesModel>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{territoryId}")]
        public virtual async Task<ActionResult<TerritoriesModel>> GetById(string territoryId)
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
        public virtual async Task<ActionResult<TerritoriesModel>> UpdateAsync(string territoryId, [FromBody] TerritoriesModel model)
        {
            if (model.TerritoryId != territoryId)
                return BadRequest("Key mismatch between route and payload");

            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<TerritoriesModel>> AddASync([FromBody] TerritoriesModel model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { result.TerritoryId }, result);
        }
    }
}