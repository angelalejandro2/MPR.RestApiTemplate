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
    public partial class CountryController : ControllerBase
    {
        private readonly CountryService _service;

        public CountryController(CountryService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<CountryDto>>> GetAsync([FromQuery] string filters = null, [FromQuery] string includes = null)
        {
            var result = await _service.GetAsync(filters, includes);

            return Ok(result);
        }

        [HttpGet("{country_Id}")]
        public virtual async Task<ActionResult<CountryDto>> GetById(string country_Id, [FromQuery] string includes = null)
        {
            var result = await _service.GetByIdAsync(country_Id, includes);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{country_Id}")]
        public virtual async Task<ActionResult> DeleteAsync(string country_Id)
        {
            await _service.DeleteAsync(country_Id);

            return NoContent();
        }

        [HttpPut("{country_Id}")]
        public virtual async Task<ActionResult<CountryDto>> UpdateAsync(string country_Id, [FromBody] CountryUpdateDto model)
        {
            if (model.Country_Id != country_Id)
                return BadRequest("Key mismatch between route and payload");
            var result = await _service.UpdateAsync(model);

            return Accepted(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<CountryDto>> AddAsync([FromBody] CountryCreateDto model)
        {
            var result = await _service.AddAsync(model);

            return CreatedAtAction(nameof(GetById), new { Country_Id = result.Country_Id }, result);
        }
    }
}
