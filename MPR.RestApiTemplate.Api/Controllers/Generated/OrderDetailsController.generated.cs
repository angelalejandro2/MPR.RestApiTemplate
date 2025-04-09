using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Application.Services;

namespace MPR.RestApiTemplate.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class OrderDetailsController : ControllerBase
    {
        private readonly OrderDetailsService _service;

        public OrderDetailsController(OrderDetailsService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<OrderDetailsDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{orderId}/{productId}")]
        public virtual async Task<ActionResult<OrderDetailsDto>> GetById(int orderId, int productId)
        {
            var result = await _service.GetByIdAsync(orderId, productId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{orderId}/{productId}")]
        public virtual async Task<ActionResult> DeleteAsync(int orderId, int productId)
        {
            await _service.DeleteAsync(orderId, productId);
            return NoContent();
        }

        [HttpPut("{orderId}/{productId}")]
        public virtual async Task<ActionResult<OrderDetailsDto>> UpdateAsync(int orderId, int productId, [FromBody] OrderDetailsUpdateDto model)
        {
            if (model.OrderId != orderId)
                return BadRequest("Key mismatch between route and payload");
            if (model.ProductId != productId)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<OrderDetailsDto>> AddAsync([FromBody] OrderDetailsCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { OrderId = result.OrderId, ProductId = result.ProductId }, result);
        }
    }
}
