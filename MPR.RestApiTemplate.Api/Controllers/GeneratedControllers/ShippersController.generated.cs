using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Application.Services;

namespace MPR.RestApiTemplate.Api.Controllers
{
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
}
