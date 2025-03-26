using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.Models;
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
        public virtual async Task<ActionResult<IEnumerable<CustomerModel>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public virtual async Task<ActionResult<CustomerModel>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> DeleteAsync(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
        [HttpPut]
        public virtual async Task<ActionResult<CustomerModel>> UpdateAsync([FromBody] CustomerModel model)
        {
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<CustomerModel>> AddASync([FromBody] CustomerModel model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { model.Id }, model);
        }
    }
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class CustomerTypeController : ControllerBase
    {
        private readonly CustomerTypeService _service;

        public CustomerTypeController(CustomerTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<CustomerTypeModel>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public virtual async Task<ActionResult<CustomerTypeModel>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> DeleteAsync(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
        [HttpPut]
        public virtual async Task<ActionResult<CustomerTypeModel>> UpdateAsync([FromBody] CustomerTypeModel model)
        {
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<CustomerTypeModel>> AddASync([FromBody] CustomerTypeModel model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { model.Id }, model);
        }
    }
}