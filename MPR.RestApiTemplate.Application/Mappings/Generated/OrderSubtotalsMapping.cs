// Autogenerated Code - Do not modify
using AutoMapper;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Domain.Entities;

namespace MPR.RestApiTemplate.Application.DTOs
{
    public partial class OrderSubtotalsMapping : Profile
    {
        public OrderSubtotalsMapping()
        {
            ConfigureMappings();
        }

        protected virtual void ConfigureMappings()
        {
            CreateMap<OrderSubtotals, OrderSubtotalsDto>().ReverseMap();
        }
    }
}
