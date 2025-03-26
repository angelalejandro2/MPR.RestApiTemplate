using AutoMapper;
using MPR.RestApiTemplate.Application.Models;
using MPR.RestApiTemplate.Domain.Entities;

namespace MPR.RestApiTemplate.Application.Models
{
	public class CustomerMapping : Profile
	{
		public CustomerMapping()
		{
			CreateMap<Customer, CustomerModel>().ReverseMap();
		}
	}
	public class CustomerTypeMapping : Profile
	{
		public CustomerTypeMapping()
		{
			CreateMap<CustomerType, CustomerTypeModel>().ReverseMap();
		}
	}
}