using Microsoft.AspNetCore.Mvc;
using MPR.RestApiTemplate.Application.Dtos;

namespace MPR.RestApiTemplate.Api.Controllers
{
    public partial class EmployeeController : ControllerBase
    {
        [HttpGet("RawQuerySample/{departmentId}")]
        public async Task<ActionResult<List<EmployeeRawQueryDto>>> ExecuteRawQueryExampleAsync(int departmentId)
        {
            var result = await _service.EmployeeRawQueryExampleAsync(departmentId);
            return Ok(result);
        }

        [HttpGet("SampleProcedureExecute/{managerId}")]
        public async Task<ActionResult<List<EmployeeSampleProcedureDto>>> ExecuteSampleProcedureAsync(int managerId)
        {
            var result = await _service.EmployeeSampleProcedureExecuteAsync(managerId);
            return Ok(result);
        }
    }
}
