using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Application.Services;

namespace MPR.RestApiTemplate.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class ProductsController : ControllerBase
    {
        private readonly ProductsService _service;

        public ProductsController(ProductsService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<ProductsDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{productId}")]
        public virtual async Task<ActionResult<ProductsDto>> GetById(int productId)
        {
            var result = await _service.GetByIdAsync(productId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{productId}")]
        public virtual async Task<ActionResult> DeleteAsync(int productId)
        {
            await _service.DeleteAsync(productId);
            return NoContent();
        }

        [HttpPut("{productId}")]
        public virtual async Task<ActionResult<ProductsDto>> UpdateAsync(int productId, [FromBody] ProductsUpdateDto model)
        {
            if (model.ProductId != productId)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<ProductsDto>> AddAsync([FromBody] ProductsCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { ProductId = result.ProductId }, result);
        }
    }
}
