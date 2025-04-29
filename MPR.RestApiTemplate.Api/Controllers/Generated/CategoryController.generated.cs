using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Application.Services;

namespace MPR.RestApiTemplate.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class CategoryController : ControllerBase
    {
        private readonly CategoryService _service;

        public CategoryController(CategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{categoryID}")]
        public virtual async Task<ActionResult<CategoryDto>> GetById(int categoryID)
        {
            var result = await _service.GetByIdAsync(categoryID);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{categoryID}")]
        public virtual async Task<ActionResult> DeleteAsync(int categoryID)
        {
            await _service.DeleteAsync(categoryID);
            return NoContent();
        }

        [HttpPut("{categoryID}")]
        public virtual async Task<ActionResult<CategoryDto>> UpdateAsync(int categoryID, [FromBody] CategoryUpdateDto model)
        {
            if (model.CategoryID != categoryID)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<CategoryDto>> AddAsync([FromBody] CategoryCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { CategoryID = result.CategoryID }, result);
        }
    }
}
