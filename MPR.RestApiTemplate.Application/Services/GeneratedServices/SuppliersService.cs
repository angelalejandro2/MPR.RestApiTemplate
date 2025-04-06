// Autogenerated Code - Do not modify
using AutoMapper;
using MPR.RestApiTemplate.Application.DTOs;
using MPR.RestApiTemplate.Domain.Entities;
using MPR.RestApiTemplate.Domain.Interfaces;
using System.Linq.Expressions;

namespace MPR.RestApiTemplate.Application.Services
{
    public partial class SuppliersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SuppliersService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public virtual async Task<IEnumerable<SuppliersDto>> GetAllAsync(params Expression<Func<Suppliers, object>>[] includes)
        {
            var entities = await _unitOfWork.SuppliersRepository.GetAllAsync(includes);
            return _mapper.Map<IEnumerable<SuppliersDto>>(entities);
        }

        public virtual async Task<SuppliersDto> GetByIdAsync(int supplierId, params Expression<Func<Suppliers, object>>[] includes)
        {
            var entity = await _unitOfWork.SuppliersRepository.GetByIdAsync(new object[] { supplierId }, includes);
            return _mapper.Map<SuppliersDto>(entity);
        }

        public virtual async Task DeleteAsync(int supplierId)
        {
            await _unitOfWork.SuppliersRepository.DeleteAsync(supplierId);
            await _unitOfWork.SaveChangesAsync();
        }

        public virtual async Task<SuppliersDto> AddAsync(SuppliersCreateDto model)
        {
            var entity = _mapper.Map<Suppliers>(model);
            entity = await _unitOfWork.SuppliersRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<SuppliersDto>(entity);
        }

        public virtual async Task<SuppliersDto> UpdateAsync(SuppliersUpdateDto model)
        {
            var entity = _mapper.Map<Suppliers>(model);
            entity = await _unitOfWork.SuppliersRepository.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<SuppliersDto>(entity);
        }
    }
}
