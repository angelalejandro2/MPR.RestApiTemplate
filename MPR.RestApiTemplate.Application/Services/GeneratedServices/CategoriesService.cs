// Autogenerated Code - Do not modify
using AutoMapper;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Domain.Entities;
using MPR.RestApiTemplate.Domain.Interfaces;
using System.Linq.Expressions;

namespace MPR.RestApiTemplate.Application.Services
{
    public partial class CategoriesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoriesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public virtual async Task<IEnumerable<CategoriesDto>> GetAllAsync(params Expression<Func<Categories, object>>[] includes)
        {
            var entities = await _unitOfWork.CategoriesRepository.GetAllAsync(includes);
            return _mapper.Map<IEnumerable<CategoriesDto>>(entities);
        }

        public virtual async Task<CategoriesDto> GetByIdAsync(int categoryId, params Expression<Func<Categories, object>>[] includes)
        {
            var entity = await _unitOfWork.CategoriesRepository.GetByIdAsync(new object[] { categoryId }, includes);
            return _mapper.Map<CategoriesDto>(entity);
        }

        public virtual async Task DeleteAsync(int categoryId)
        {
            await _unitOfWork.CategoriesRepository.DeleteAsync(categoryId);
            await _unitOfWork.SaveChangesAsync();
        }

        public virtual async Task<CategoriesDto> AddAsync(CategoriesCreateDto model)
        {
            var entity = _mapper.Map<Categories>(model);
            entity = await _unitOfWork.CategoriesRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CategoriesDto>(entity);
        }

        public virtual async Task<CategoriesDto> UpdateAsync(CategoriesUpdateDto model)
        {
            var entity = _mapper.Map<Categories>(model);
            entity = await _unitOfWork.CategoriesRepository.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CategoriesDto>(entity);
        }
    }
}
