using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Application.Services;

namespace MPR.RestApiTemplate.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class TerritoriesController : ControllerBase
    {
        private readonly TerritoriesService _service;

        public TerritoriesController(TerritoriesService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TerritoriesDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{territoryId}")]
        public virtual async Task<ActionResult<TerritoriesDto>> GetById(string territoryId)
        {
            var result = await _service.GetByIdAsync(territoryId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{territoryId}")]
        public virtual async Task<ActionResult> DeleteAsync(string territoryId)
        {
            await _service.DeleteAsync(territoryId);
            return NoContent();
        }

        [HttpPut("{territoryId}")]
        public virtual async Task<ActionResult<TerritoriesDto>> UpdateAsync(string territoryId, [FromBody] TerritoriesUpdateDto model)
        {
            if (model.TerritoryId != territoryId)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<TerritoriesDto>> AddAsync([FromBody] TerritoriesCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { TerritoryId = result.TerritoryId }, result);
        }
    }
}
