using MPR.RestApiTemplate.Domain.Interfaces.Repositories;

namespace MPR.RestApiTemplate.Domain.Interfaces
{
	public partial interface IUnitOfWork : IDisposable
	{
		// TODO: Add repository interfaces here

		Task<int> SaveChangesAsync();
	}
}