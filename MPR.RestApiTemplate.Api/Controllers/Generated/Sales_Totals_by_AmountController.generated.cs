using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Application.Services;

namespace MPR.RestApiTemplate.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class Sales_Totals_by_AmountController : ControllerBase
    {
        private readonly Sales_Totals_by_AmountService _service;

        public Sales_Totals_by_AmountController(Sales_Totals_by_AmountService service)
        {
            _service = service;
        }

        [Authorize(Policy = "GetSales_Totals_by_Amount")]
        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<Sales_Totals_by_AmountDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

    }
}
