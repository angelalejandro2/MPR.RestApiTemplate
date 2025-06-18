using MPR.RestApiTemplate.Domain.Interfaces.Repositories;

namespace MPR.RestApiTemplate.Domain.Interfaces
{
	public partial interface IUnitOfWork : IDisposable
	{
		ICountryRepository CountryRepository { get; }
		IDepartmentRepository DepartmentRepository { get; }
		IEmpDetailsViewRepository EmpDetailsViewRepository { get; }
		IEmployeeRepository EmployeeRepository { get; }
		IJobRepository JobRepository { get; }
		IJobHistoryRepository JobHistoryRepository { get; }
		ILocationRepository LocationRepository { get; }
		IRegionRepository RegionRepository { get; }
		IHrContextSqlExecutor HrContextSqlExecutor { get; }

		Task<int> SaveChangesAsync();
	}
}