using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Application.Services;

namespace MPR.RestApiTemplate.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class SupplierController : ControllerBase
    {
        private readonly SupplierService _service;

        public SupplierController(SupplierService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<SupplierDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{supplierID}")]
        public virtual async Task<ActionResult<SupplierDto>> GetById(int supplierID)
        {
            var result = await _service.GetByIdAsync(supplierID);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{supplierID}")]
        public virtual async Task<ActionResult> DeleteAsync(int supplierID)
        {
            await _service.DeleteAsync(supplierID);
            return NoContent();
        }

        [HttpPut("{supplierID}")]
        public virtual async Task<ActionResult<SupplierDto>> UpdateAsync(int supplierID, [FromBody] SupplierUpdateDto model)
        {
            if (model.SupplierID != supplierID)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<SupplierDto>> AddAsync([FromBody] SupplierCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { SupplierID = result.SupplierID }, result);
        }
    }
}
