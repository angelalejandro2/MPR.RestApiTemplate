using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Application.Services;

namespace MPR.RestApiTemplate.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class OrdersController : ControllerBase
    {
        private readonly OrdersService _service;

        public OrdersController(OrdersService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<OrdersDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{orderId}")]
        public virtual async Task<ActionResult<OrdersDto>> GetById(int orderId)
        {
            var result = await _service.GetByIdAsync(orderId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{orderId}")]
        public virtual async Task<ActionResult> DeleteAsync(int orderId)
        {
            await _service.DeleteAsync(orderId);
            return NoContent();
        }

        [HttpPut("{orderId}")]
        public virtual async Task<ActionResult<OrdersDto>> UpdateAsync(int orderId, [FromBody] OrdersUpdateDto model)
        {
            if (model.OrderId != orderId)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<OrdersDto>> AddAsync([FromBody] OrdersCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { OrderId = result.OrderId }, result);
        }
    }
}
