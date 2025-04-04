using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Application.Services;

namespace MPR.RestApiTemplate.Api.Controllers
{
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
}
