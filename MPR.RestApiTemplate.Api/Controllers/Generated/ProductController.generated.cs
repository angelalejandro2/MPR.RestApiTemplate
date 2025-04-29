using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Application.Services;

namespace MPR.RestApiTemplate.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class ProductController : ControllerBase
    {
        private readonly ProductService _service;

        public ProductController(ProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<ProductDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{productID}")]
        public virtual async Task<ActionResult<ProductDto>> GetById(int productID)
        {
            var result = await _service.GetByIdAsync(productID);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{productID}")]
        public virtual async Task<ActionResult> DeleteAsync(int productID)
        {
            await _service.DeleteAsync(productID);
            return NoContent();
        }

        [HttpPut("{productID}")]
        public virtual async Task<ActionResult<ProductDto>> UpdateAsync(int productID, [FromBody] ProductUpdateDto model)
        {
            if (model.ProductID != productID)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<ProductDto>> AddAsync([FromBody] ProductCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { ProductID = result.ProductID }, result);
        }
    }
}
