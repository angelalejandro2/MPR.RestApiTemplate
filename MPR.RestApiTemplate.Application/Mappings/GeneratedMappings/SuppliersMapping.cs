// Autogenerated Code - Do not modify
using AutoMapper;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Domain.Entities;

namespace MPR.RestApiTemplate.Application.DTOs
{
    public partial class SuppliersMapping : Profile
    {
        public SuppliersMapping()
        {
            ConfigureMappings();
        }

        protected virtual void ConfigureMappings()
        {
            CreateMap<Suppliers, SuppliersDto>().ReverseMap();
            CreateMap<Suppliers, SuppliersCreateDto>().ReverseMap();
            CreateMap<Suppliers, SuppliersUpdateDto>().ReverseMap();
        }
    }
}
