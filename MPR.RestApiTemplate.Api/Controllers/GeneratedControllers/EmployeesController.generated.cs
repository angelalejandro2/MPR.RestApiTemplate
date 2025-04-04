using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Application.Services;

namespace MPR.RestApiTemplate.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class EmployeesController : ControllerBase
    {
        private readonly EmployeesService _service;

        public EmployeesController(EmployeesService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<EmployeesDto>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{employeeId}")]
        public virtual async Task<ActionResult<EmployeesDto>> GetById(int employeeId)
        {
            var result = await _service.GetByIdAsync(employeeId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{employeeId}")]
        public virtual async Task<ActionResult> DeleteAsync(int employeeId)
        {
            await _service.DeleteAsync(employeeId);
            return NoContent();
        }

        [HttpPut("{employeeId}")]
        public virtual async Task<ActionResult<EmployeesDto>> UpdateAsync(int employeeId, [FromBody] EmployeesUpdateDto model)
        {
            if (model.EmployeeId != employeeId)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);
            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<EmployeesDto>> AddAsync([FromBody] EmployeesCreateDto model)
        {
            var result = await _service.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { EmployeeId = result.EmployeeId }, result);
        }
    }
}
