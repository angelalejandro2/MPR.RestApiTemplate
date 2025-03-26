using MPR.RestApiTemplate.Domain.Entities;

namespace MPR.RestApiTemplate.Domain.Interfaces.Repositories
{
	public interface ICustomerRepository : IGenericRepository<Customer>	{ }
	public interface ICustomerTypeRepository : IGenericRepository<CustomerType>	{ }
}