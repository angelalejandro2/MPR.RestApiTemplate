using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Application.Services;

namespace MPR.RestApiTemplate.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class OrderController : ControllerBase
    {
        private readonly OrderService _service;

        public OrderController(OrderService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<OrderDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{orderID}")]
        public virtual async Task<ActionResult<OrderDto>> GetById(int orderID)
        {
            var result = await _service.GetByIdAsync(orderID);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{orderID}")]
        public virtual async Task<ActionResult> DeleteAsync(int orderID)
        {
            await _service.DeleteAsync(orderID);
            return NoContent();
        }

        [HttpPut("{orderID}")]
        public virtual async Task<ActionResult<OrderDto>> UpdateAsync(int orderID, [FromBody] OrderUpdateDto model)
        {
            if (model.OrderID != orderID)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<OrderDto>> AddAsync([FromBody] OrderCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { OrderID = result.OrderID }, result);
        }
    }
}
