using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Application.Services;

namespace MPR.RestApiTemplate.Api.Controllers
{
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
}
