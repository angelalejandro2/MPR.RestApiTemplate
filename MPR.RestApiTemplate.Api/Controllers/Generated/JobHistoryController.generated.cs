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
    public partial class JobHistoryController : ControllerBase
    {
        private readonly JobHistoryService _service;

        public JobHistoryController(JobHistoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<JobHistoryDto>>> GetAsync([FromQuery] string filters = null, [FromQuery] string includes = null)
        {
            var result = await _service.GetAsync(filters, includes);

            return Ok(result);
        }

        [HttpGet("{employee_Id}/{start_Date}")]
        public virtual async Task<ActionResult<JobHistoryDto>> GetById(int employee_Id, DateTime start_Date, [FromQuery] string includes = null)
        {
            var result = await _service.GetByIdAsync(employee_Id, start_Date, includes);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{employee_Id}/{start_Date}")]
        public virtual async Task<ActionResult> DeleteAsync(int employee_Id, DateTime start_Date)
        {
            await _service.DeleteAsync(employee_Id, start_Date);

            return NoContent();
        }

        [HttpPut("{employee_Id}/{start_Date}")]
        public virtual async Task<ActionResult<JobHistoryDto>> UpdateAsync(int employee_Id, DateTime start_Date, [FromBody] JobHistoryUpdateDto model)
        {
            if (model.Employee_Id != employee_Id)
                return BadRequest("Key mismatch between route and payload");
            if (model.Start_Date != start_Date)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);

            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<JobHistoryDto>> AddAsync([FromBody] JobHistoryCreateDto model)
        {
            var result = await _service.AddAsync(model);

            return CreatedAtAction(nameof(GetById), new { Employee_Id = result.Employee_Id, Start_Date = result.Start_Date }, result);
        }
    }
}
