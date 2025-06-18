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
    public partial class RegionController : ControllerBase
    {
        private readonly RegionService _service;

        public RegionController(RegionService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<RegionDto>>> GetAsync([FromQuery] string filters = null, [FromQuery] string includes = null)
        {
            var result = await _service.GetAsync(filters, includes);

            return Ok(result);
        }

        [HttpGet("{region_Id}")]
        public virtual async Task<ActionResult<RegionDto>> GetById(decimal region_Id, [FromQuery] string includes = null)
        {
            var result = await _service.GetByIdAsync(region_Id, includes);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{region_Id}")]
        public virtual async Task<ActionResult> DeleteAsync(decimal region_Id)
        {
            await _service.DeleteAsync(region_Id);

            return NoContent();
        }

        [HttpPut("{region_Id}")]
        public virtual async Task<ActionResult<RegionDto>> UpdateAsync(decimal region_Id, [FromBody] RegionUpdateDto model)
        {
            if (model.Region_Id != region_Id)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);

            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<RegionDto>> AddAsync([FromBody] RegionCreateDto model)
        {
            var result = await _service.AddAsync(model);

            return CreatedAtAction(nameof(GetById), new { Region_Id = result.Region_Id }, result);
        }
    }
}
