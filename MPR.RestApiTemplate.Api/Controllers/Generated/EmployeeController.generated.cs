using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Application.Services;

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
        public virtual async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{employeeID}")]
        public virtual async Task<ActionResult<EmployeeDto>> GetById(int employeeID)
        {
            var result = await _service.GetByIdAsync(employeeID);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{employeeID}")]
        public virtual async Task<ActionResult> DeleteAsync(int employeeID)
        {
            await _service.DeleteAsync(employeeID);
            return NoContent();
        }

        [HttpPut("{employeeID}")]
        public virtual async Task<ActionResult<EmployeeDto>> UpdateAsync(int employeeID, [FromBody] EmployeeUpdateDto model)
        {
            if (model.EmployeeID != employeeID)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<EmployeeDto>> AddAsync([FromBody] EmployeeCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { EmployeeID = result.EmployeeID }, result);
        }
    }
}
