using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Application.Services;

namespace MPR.RestApiTemplate.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class CustomerDemographicController : ControllerBase
    {
        private readonly CustomerDemographicService _service;

        public CustomerDemographicController(CustomerDemographicService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<CustomerDemographicDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{customerTypeID}")]
        public virtual async Task<ActionResult<CustomerDemographicDto>> GetById(string customerTypeID)
        {
            var result = await _service.GetByIdAsync(customerTypeID);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{customerTypeID}")]
        public virtual async Task<ActionResult> DeleteAsync(string customerTypeID)
        {
            await _service.DeleteAsync(customerTypeID);
            return NoContent();
        }

        [HttpPut("{customerTypeID}")]
        public virtual async Task<ActionResult<CustomerDemographicDto>> UpdateAsync(string customerTypeID, [FromBody] CustomerDemographicUpdateDto model)
        {
            if (model.CustomerTypeID != customerTypeID)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<CustomerDemographicDto>> AddAsync([FromBody] CustomerDemographicCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { CustomerTypeID = result.CustomerTypeID }, result);
        }
    }
}
