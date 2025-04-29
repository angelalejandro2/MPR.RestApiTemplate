using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Application.Services;

namespace MPR.RestApiTemplate.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class ShipperController : ControllerBase
    {
        private readonly ShipperService _service;

        public ShipperController(ShipperService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<ShipperDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{shipperID}")]
        public virtual async Task<ActionResult<ShipperDto>> GetById(int shipperID)
        {
            var result = await _service.GetByIdAsync(shipperID);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{shipperID}")]
        public virtual async Task<ActionResult> DeleteAsync(int shipperID)
        {
            await _service.DeleteAsync(shipperID);
            return NoContent();
        }

        [HttpPut("{shipperID}")]
        public virtual async Task<ActionResult<ShipperDto>> UpdateAsync(int shipperID, [FromBody] ShipperUpdateDto model)
        {
            if (model.ShipperID != shipperID)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<ShipperDto>> AddAsync([FromBody] ShipperCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { ShipperID = result.ShipperID }, result);
        }
    }
}
