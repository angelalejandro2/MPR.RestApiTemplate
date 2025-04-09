// Autogenerated Code - Do not modify
using AutoMapper;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Domain.Entities;
using MPR.RestApiTemplate.Domain.Interfaces;
using System.Linq.Expressions;

namespace MPR.RestApiTemplate.Application.Services
{
    public partial class AlphabeticalListOfProductsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AlphabeticalListOfProductsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public virtual async Task<IEnumerable<AlphabeticalListOfProductsDto>> GetAllAsync(params Expression<Func<AlphabeticalListOfProducts, object>>[] includes)
        {
            var entities = await _unitOfWork.AlphabeticalListOfProductsRepository.GetAllAsync(includes);
            return _mapper.Map<IEnumerable<AlphabeticalListOfProductsDto>>(entities);
        }
    }
}
