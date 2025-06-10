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
    public partial class Alphabetical_list_of_productController : ControllerBase
    {
        private readonly Alphabetical_list_of_productService _service;

        public Alphabetical_list_of_productController(Alphabetical_list_of_productService service)
        {
            _service = service;
        }

        [Authorize(Policy = "GetAlphabetical_list_of_product")]
        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<Alphabetical_list_of_productDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

    }
}
