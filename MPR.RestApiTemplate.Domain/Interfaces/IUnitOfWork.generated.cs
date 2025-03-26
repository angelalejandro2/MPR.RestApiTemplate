using MPR.RestApiTemplate.Domain.Interfaces.Repositories;

namespace MPR.RestApiTemplate.Domain.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		ICustomerRepository CustomerRepository { get; }
		ICustomerTypeRepository CustomerTypeRepository { get; }

		Task<int> SaveChangesAsync();
	}
}