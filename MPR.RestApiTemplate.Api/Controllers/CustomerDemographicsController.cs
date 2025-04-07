using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Application.Services;

namespace MPR.RestApiTemplate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public partial class CustomerDemographics2Controller : CustomerDemographicsController
    {
        private readonly CustomerDemographicsService _service;
        public CustomerDemographics2Controller(CustomerDemographicsService service) : base(service)
        {
            _service = service;
        }

        public override async Task<ActionResult<IEnumerable<CustomerDemographicsDto>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        [HttpGet("Dummy")]
        [ApiExplorerSettings(IgnoreApi = false)]
        public IActionResult SomethingAsync()
        { 
            return Ok(); 
        }
    }
}
