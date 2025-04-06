// Autogenerated Code - Do not modify
using AutoMapper;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Domain.Entities;
using MPR.RestApiTemplate.Domain.Interfaces;
using System.Linq.Expressions;

namespace MPR.RestApiTemplate.Application.Services
{
    public partial class ProductsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public virtual async Task<IEnumerable<ProductsDto>> GetAllAsync(params Expression<Func<Products, object>>[] includes)
        {
            var entities = await _unitOfWork.ProductsRepository.GetAllAsync(includes);
            return _mapper.Map<IEnumerable<ProductsDto>>(entities);
        }

        public virtual async Task<ProductsDto> GetByIdAsync(int productId, params Expression<Func<Products, object>>[] includes)
        {
            var entity = await _unitOfWork.ProductsRepository.GetByIdAsync(new object[] { productId }, includes);
            return _mapper.Map<ProductsDto>(entity);
        }

        public virtual async Task DeleteAsync(int productId)
        {
            await _unitOfWork.ProductsRepository.DeleteAsync(productId);
            await _unitOfWork.SaveChangesAsync();
        }

        public virtual async Task<ProductsDto> AddAsync(ProductsCreateDto model)
        {
            var entity = _mapper.Map<Products>(model);
            entity = await _unitOfWork.ProductsRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ProductsDto>(entity);
        }

        public virtual async Task<ProductsDto> UpdateAsync(ProductsUpdateDto model)
        {
            var entity = _mapper.Map<Products>(model);
            entity = await _unitOfWork.ProductsRepository.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ProductsDto>(entity);
        }
    }
}
