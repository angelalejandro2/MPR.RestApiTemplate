using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Application.Services;

namespace MPR.RestApiTemplate.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class CategoriesController : ControllerBase
    {
        private readonly CategoriesService _service;

        public CategoriesController(CategoriesService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<CategoriesDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{categoryId}")]
        public virtual async Task<ActionResult<CategoriesDto>> GetById(int categoryId)
        {
            var result = await _service.GetByIdAsync(categoryId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{categoryId}")]
        public virtual async Task<ActionResult> DeleteAsync(int categoryId)
        {
            await _service.DeleteAsync(categoryId);
            return NoContent();
        }

        [HttpPut("{categoryId}")]
        public virtual async Task<ActionResult<CategoriesDto>> UpdateAsync(int categoryId, [FromBody] CategoriesUpdateDto model)
        {
            if (model.CategoryId != categoryId)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<CategoriesDto>> AddAsync([FromBody] CategoriesCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { CategoryId = result.CategoryId }, result);
        }
    }
}
