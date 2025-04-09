using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Application.Services;

namespace MPR.RestApiTemplate.Api.Controllers
{
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
}
