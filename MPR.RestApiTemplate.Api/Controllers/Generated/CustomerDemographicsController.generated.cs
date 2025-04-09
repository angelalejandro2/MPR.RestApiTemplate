using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Application.Services;

namespace MPR.RestApiTemplate.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class CustomerDemographicsController : ControllerBase
    {
        private readonly CustomerDemographicsService _service;

        public CustomerDemographicsController(CustomerDemographicsService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<CustomerDemographicsDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{customerTypeId}")]
        public virtual async Task<ActionResult<CustomerDemographicsDto>> GetById(string customerTypeId)
        {
            var result = await _service.GetByIdAsync(customerTypeId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{customerTypeId}")]
        public virtual async Task<ActionResult> DeleteAsync(string customerTypeId)
        {
            await _service.DeleteAsync(customerTypeId);
            return NoContent();
        }

        [HttpPut("{customerTypeId}")]
        public virtual async Task<ActionResult<CustomerDemographicsDto>> UpdateAsync(string customerTypeId, [FromBody] CustomerDemographicsUpdateDto model)
        {
            if (model.CustomerTypeId != customerTypeId)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<CustomerDemographicsDto>> AddAsync([FromBody] CustomerDemographicsCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { CustomerTypeId = result.CustomerTypeId }, result);
        }
    }
}
