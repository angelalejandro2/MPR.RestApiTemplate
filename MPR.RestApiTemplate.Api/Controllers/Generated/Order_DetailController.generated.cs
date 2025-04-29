using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Application.Services;

namespace MPR.RestApiTemplate.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class Order_DetailController : ControllerBase
    {
        private readonly Order_DetailService _service;

        public Order_DetailController(Order_DetailService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<Order_DetailDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{orderID}/{productID}")]
        public virtual async Task<ActionResult<Order_DetailDto>> GetById(int orderID, int productID)
        {
            var result = await _service.GetByIdAsync(orderID, productID);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{orderID}/{productID}")]
        public virtual async Task<ActionResult> DeleteAsync(int orderID, int productID)
        {
            await _service.DeleteAsync(orderID, productID);
            return NoContent();
        }

        [HttpPut("{orderID}/{productID}")]
        public virtual async Task<ActionResult<Order_DetailDto>> UpdateAsync(int orderID, int productID, [FromBody] Order_DetailUpdateDto model)
        {
            if (model.OrderID != orderID)
                return BadRequest("Key mismatch between route and payload");
            if (model.ProductID != productID)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<Order_DetailDto>> AddAsync([FromBody] Order_DetailCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { OrderID = result.OrderID, ProductID = result.ProductID }, result);
        }
    }
}
