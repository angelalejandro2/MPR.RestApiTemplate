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
    public partial class JobController : ControllerBase
    {
        private readonly JobService _service;

        public JobController(JobService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<JobDto>>> GetAsync([FromQuery] string filters = null, [FromQuery] string includes = null)
        {
            var result = await _service.GetAsync(filters, includes);

            return Ok(result);
        }

        [HttpGet("{job_Id}")]
        public virtual async Task<ActionResult<JobDto>> GetById(string job_Id, [FromQuery] string includes = null)
        {
            var result = await _service.GetByIdAsync(job_Id, includes);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{job_Id}")]
        public virtual async Task<ActionResult> DeleteAsync(string job_Id)
        {
            await _service.DeleteAsync(job_Id);

            return NoContent();
        }

        [HttpPut("{job_Id}")]
        public virtual async Task<ActionResult<JobDto>> UpdateAsync(string job_Id, [FromBody] JobUpdateDto model)
        {
            if (model.Job_Id != job_Id)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);

            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<JobDto>> AddAsync([FromBody] JobCreateDto model)
        {
            var result = await _service.AddAsync(model);

            return CreatedAtAction(nameof(GetById), new { Job_Id = result.Job_Id }, result);
        }
    }
}
