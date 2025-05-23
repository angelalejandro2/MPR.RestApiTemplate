// Autogenerated Code - Do not modify
using AutoMapper;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Domain.Entities;
using MPR.RestApiTemplate.Domain.Interfaces;
using System.Linq.Expressions;

namespace MPR.RestApiTemplate.Application.Services
{
    public partial class CustomerDemographicsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerDemographicsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public virtual async Task<IEnumerable<CustomerDemographicsDto>> GetAllAsync(params Expression<Func<CustomerDemographics, object>>[] includes)
        {
            var entities = await _unitOfWork.CustomerDemographicsRepository.GetAllAsync(includes);
            return _mapper.Map<IEnumerable<CustomerDemographicsDto>>(entities);
        }

        public virtual async Task<CustomerDemographicsDto> GetByIdAsync(string customerTypeId, params Expression<Func<CustomerDemographics, object>>[] includes)
        {
            var entity = await _unitOfWork.CustomerDemographicsRepository.GetByIdAsync(new object[] { customerTypeId }, includes);
            return _mapper.Map<CustomerDemographicsDto>(entity);
        }

        public virtual async Task DeleteAsync(string customerTypeId)
        {
            await _unitOfWork.CustomerDemographicsRepository.DeleteAsync(customerTypeId);
            await _unitOfWork.SaveChangesAsync();
        }

        public virtual async Task<CustomerDemographicsDto> AddAsync(CustomerDemographicsCreateDto model)
        {
            var entity = _mapper.Map<CustomerDemographics>(model);
            entity = await _unitOfWork.CustomerDemographicsRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CustomerDemographicsDto>(entity);
        }

        public virtual async Task<CustomerDemographicsDto> UpdateAsync(CustomerDemographicsUpdateDto model)
        {
            var entity = _mapper.Map<CustomerDemographics>(model);
            entity = await _unitOfWork.CustomerDemographicsRepository.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CustomerDemographicsDto>(entity);
        }
    }
}
