// Autogenerated Code - Do not modify
using AutoMapper;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Domain.Entities;

namespace MPR.RestApiTemplate.Application.DTOs
{
    public partial class SummaryOfSalesByYearMapping : Profile
    {
        public SummaryOfSalesByYearMapping()
        {
            ConfigureMappings();
        }

        protected virtual void ConfigureMappings()
        {
            CreateMap<SummaryOfSalesByYear, SummaryOfSalesByYearDto>().ReverseMap();
        }
    }
}
