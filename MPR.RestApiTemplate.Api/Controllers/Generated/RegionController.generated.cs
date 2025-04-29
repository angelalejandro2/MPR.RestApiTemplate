using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Application.Services;

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
        public virtual async Task<ActionResult<IEnumerable<RegionDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{regionID}")]
        public virtual async Task<ActionResult<RegionDto>> GetById(int regionID)
        {
            var result = await _service.GetByIdAsync(regionID);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{regionID}")]
        public virtual async Task<ActionResult> DeleteAsync(int regionID)
        {
            await _service.DeleteAsync(regionID);
            return NoContent();
        }

        [HttpPut("{regionID}")]
        public virtual async Task<ActionResult<RegionDto>> UpdateAsync(int regionID, [FromBody] RegionUpdateDto model)
        {
            if (model.RegionID != regionID)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<RegionDto>> AddAsync([FromBody] RegionCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { RegionID = result.RegionID }, result);
        }
    }
}
