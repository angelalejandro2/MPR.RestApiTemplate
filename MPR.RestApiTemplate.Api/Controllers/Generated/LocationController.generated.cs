using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.Dtos;
using MPR.RestApiTemplate.Application.Services;
using NSwag.Annotations;

namespace MPR.RestApiTemplate.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class LocationController : ControllerBase
    {
        private readonly LocationService _service;

        public LocationController(LocationService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<LocationDto>>> GetAsync([FromQuery] string filters = null, [FromQuery] string includes = null)
        {
            var result = await _service.GetAsync(filters, includes);

            return Ok(result);
        }

        [HttpGet("{location_Id}")]
        public virtual async Task<ActionResult<LocationDto>> GetById(int location_Id, [FromQuery] string includes = null)
        {
            var result = await _service.GetByIdAsync(location_Id, includes);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{location_Id}")]
        public virtual async Task<ActionResult> DeleteAsync(int location_Id)
        {
            await _service.DeleteAsync(location_Id);

            return NoContent();
        }

        [HttpPut("{location_Id}")]
        public virtual async Task<ActionResult<LocationDto>> UpdateAsync(int location_Id, [FromBody] LocationUpdateDto model)
        {
            if (model.Location_Id != location_Id)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);

            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<LocationDto>> AddAsync([FromBody] LocationCreateDto model)
        {
            var result = await _service.AddAsync(model);

            return CreatedAtAction(nameof(GetById), new { Location_Id = result.Location_Id }, result);
        }
    }
}
