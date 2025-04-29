using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Application.Services;

namespace MPR.RestApiTemplate.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class CustomerController : ControllerBase
    {
        private readonly CustomerService _service;

        public CustomerController(CustomerService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<CustomerDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{customerID}")]
        public virtual async Task<ActionResult<CustomerDto>> GetById(string customerID)
        {
            var result = await _service.GetByIdAsync(customerID);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{customerID}")]
        public virtual async Task<ActionResult> DeleteAsync(string customerID)
        {
            await _service.DeleteAsync(customerID);
            return NoContent();
        }

        [HttpPut("{customerID}")]
        public virtual async Task<ActionResult<CustomerDto>> UpdateAsync(string customerID, [FromBody] CustomerUpdateDto model)
        {
            if (model.CustomerID != customerID)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<CustomerDto>> AddAsync([FromBody] CustomerCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { CustomerID = result.CustomerID }, result);
        }
    }
}
