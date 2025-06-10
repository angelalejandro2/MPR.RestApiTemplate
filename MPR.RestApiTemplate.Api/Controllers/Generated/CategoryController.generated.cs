using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Policy = "GetCategory")]
        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [Authorize(Policy = "GetbyidCategory")]
        [HttpGet("{categoryID}")]
        public virtual async Task<ActionResult<CategoryDto>> GetById(int categoryID)
        {
            var result = await _service.GetByIdAsync(categoryID);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [Authorize(Policy = "DeleteCategory")]
        [HttpDelete("{categoryID}")]
        public virtual async Task<ActionResult> DeleteAsync(int categoryID)
        {
            await _service.DeleteAsync(categoryID);
            return NoContent();
        }

        [Authorize(Policy = "PutCategory")]
        [HttpPut("{categoryID}")]
        public virtual async Task<ActionResult<CategoryDto>> UpdateAsync(int categoryID, [FromBody] CategoryUpdateDto model)
        {
            if (model.CategoryID != categoryID)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [Authorize(Policy = "PostCategory")]
        [HttpPost]
        public virtual async Task<ActionResult<CategoryDto>> AddAsync([FromBody] CategoryCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { CategoryID = result.CategoryID }, result);
        }

    }
}
