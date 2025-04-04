using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Application.Services;

namespace MPR.RestApiTemplate.Api.Controllers
{
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
}
