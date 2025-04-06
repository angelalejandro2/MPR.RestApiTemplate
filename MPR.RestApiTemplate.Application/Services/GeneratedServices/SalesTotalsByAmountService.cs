// Autogenerated Code - Do not modify
using AutoMapper;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Domain.Entities;
using MPR.RestApiTemplate.Domain.Interfaces;
using System.Linq.Expressions;

namespace MPR.RestApiTemplate.Application.Services
{
    public partial class SalesTotalsByAmountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SalesTotalsByAmountService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public virtual async Task<IEnumerable<SalesTotalsByAmountDto>> GetAllAsync(params Expression<Func<SalesTotalsByAmount, object>>[] includes)
        {
            var entities = await _unitOfWork.SalesTotalsByAmountRepository.GetAllAsync(includes);
            return _mapper.Map<IEnumerable<SalesTotalsByAmountDto>>(entities);
        }
    }
}
