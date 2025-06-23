using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.Dtos;
using MPR.RestApiTemplate.Application.Services;
using NSwag.Annotations;

namespace MPR.RestApiTemplate.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _service;

        public EmployeeController(EmployeeService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAsync([FromQuery] string filters = null, [FromQuery] string includes = null)
        {
            var result = await _service.GetAsync(filters, includes);

            return Ok(result);
        }

        [HttpGet("{employee_Id}")]
        public virtual async Task<ActionResult<EmployeeDto>> GetById(int employee_Id, [FromQuery] string includes = null)
        {
            var result = await _service.GetByIdAsync(employee_Id, includes);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{employee_Id}")]
        public virtual async Task<ActionResult> DeleteAsync(int employee_Id)
        {
            await _service.DeleteAsync(employee_Id);

            return NoContent();
        }

        [HttpPut("{employee_Id}")]
        public virtual async Task<ActionResult<EmployeeDto>> UpdateAsync(int employee_Id, [FromBody] EmployeeUpdateDto model)
        {
            if (model.Employee_Id != employee_Id)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);

            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<EmployeeDto>> AddAsync([FromBody] EmployeeCreateDto model)
        {
            var result = await _service.AddAsync(model);

            return CreatedAtAction(nameof(GetById), new { Employee_Id = result.Employee_Id }, result);
        }
    }
}
