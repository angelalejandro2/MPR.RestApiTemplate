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
    public partial class DepartmentController : ControllerBase
    {
        private readonly DepartmentService _service;

        public DepartmentController(DepartmentService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAsync([FromQuery] string filters = null, [FromQuery] string includes = null)
        {
            var result = await _service.GetAsync(filters, includes);

            return Ok(result);
        }

        [HttpGet("{department_Id}")]
        public virtual async Task<ActionResult<DepartmentDto>> GetById(int department_Id, [FromQuery] string includes = null)
        {
            var result = await _service.GetByIdAsync(department_Id, includes);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{department_Id}")]
        public virtual async Task<ActionResult> DeleteAsync(int department_Id)
        {
            await _service.DeleteAsync(department_Id);

            return NoContent();
        }

        [HttpPut("{department_Id}")]
        public virtual async Task<ActionResult<DepartmentDto>> UpdateAsync(int department_Id, [FromBody] DepartmentUpdateDto model)
        {
            if (model.Department_Id != department_Id)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);

            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<DepartmentDto>> AddAsync([FromBody] DepartmentCreateDto model)
        {
            var result = await _service.AddAsync(model);

            return CreatedAtAction(nameof(GetById), new { Department_Id = result.Department_Id }, result);
        }
    }
}
