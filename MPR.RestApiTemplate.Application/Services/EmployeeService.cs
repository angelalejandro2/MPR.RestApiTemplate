using System.Data;
using System.Linq.Expressions;
using System.Text;
using MPR.RestApiTemplate.Application.Dtos;
using MPR.RestApiTemplate.Domain.Interfaces.Repositories;

namespace MPR.RestApiTemplate.Application.Services
{
    public partial class EmployeeService
    {
        public async Task<List<EmployeeRawQueryDto>> EmployeeRawQueryExampleAsync(int DepartmentId)
        {
            var query = "SELECT FIRST_NAME, LAST_NAME, JOB_ID FROM EMPLOYEES WHERE DEPARTMENT_ID = :deptId";

            var result = await _unitOfWork.HrContextSqlExecutor.ExecuteSqlQueryAsync(
                query,
                reader => new EmployeeRawQueryDto
                {
                    FirstName = reader.GetString("FIRST_NAME"),
                    LastName = reader.GetString("LAST_NAME"),
                    JobId = reader.GetString("JOB_ID")
                },
                new List<DbParameterDefinition>
                {
                    new() { Name="deptId", Value=DepartmentId }
                }
            );

            return result.ToList();
        }

        public async Task<List<EmployeeSampleProcedureDto>> EmployeeSampleProcedureExecuteAsync(int ManagerId)
        {
            var result = await _unitOfWork.HrContextSqlExecutor.ExecuteStoredProcedureAsync(
                "sample_procedure",
                reader => new EmployeeSampleProcedureDto
                {
                    ManagerId = reader.GetInt32("MANAGER_ID"),
                    EmployeeCount = reader.GetInt32("EMPLOYEE_COUNT")
                },
                new List<DbParameterDefinition>
                {
                    new() { Name="p_manager_id", Value = ManagerId}
                },
                "p_result"
            );

            return result.ToList();
        }
    }
}