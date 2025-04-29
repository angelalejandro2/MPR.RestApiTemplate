using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Application.Services;

namespace MPR.RestApiTemplate.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class TerritoryController : ControllerBase
    {
        private readonly TerritoryService _service;

        public TerritoryController(TerritoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TerritoryDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{territoryID}")]
        public virtual async Task<ActionResult<TerritoryDto>> GetById(string territoryID)
        {
            var result = await _service.GetByIdAsync(territoryID);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{territoryID}")]
        public virtual async Task<ActionResult> DeleteAsync(string territoryID)
        {
            await _service.DeleteAsync(territoryID);
            return NoContent();
        }

        [HttpPut("{territoryID}")]
        public virtual async Task<ActionResult<TerritoryDto>> UpdateAsync(string territoryID, [FromBody] TerritoryUpdateDto model)
        {
            if (model.TerritoryID != territoryID)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<TerritoryDto>> AddAsync([FromBody] TerritoryCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { TerritoryID = result.TerritoryID }, result);
        }
    }
}
