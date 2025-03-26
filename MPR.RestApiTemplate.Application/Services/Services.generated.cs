using AutoMapper;
using MPR.RestApiTemplate.Application.Models;
using MPR.RestApiTemplate.Domain.Entities;
using MPR.RestApiTemplate.Domain.Interfaces;

namespace MPR.RestApiTemplate.Application.Services
{
	public partial class CustomerService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public virtual async Task<IEnumerable<CustomerModel>> GetAllAsync()
		{
			var entities = await _unitOfWork.CustomerRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<CustomerModel>>(entities);
		}

		public virtual async Task<CustomerModel> GetByIdAsync(int id)
		{
			var entity = await _unitOfWork.CustomerRepository.GetByIdAsync(id);
			return _mapper.Map<CustomerModel>(entity);
		}

		public virtual async Task DeleteAsync(int id)
		{
			await _unitOfWork.CustomerRepository.DeleteAsync(id);
			await _unitOfWork.SaveChangesAsync();
		}
		public virtual async Task<CustomerModel> AddAsync(CustomerModel model)
		{
			var entity = _mapper.Map<Customer>(model);
			entity = await _unitOfWork.CustomerRepository.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<CustomerModel>(entity);
		}

		public virtual async Task<CustomerModel> UpdateAsync(CustomerModel model)
		{
			var entity = _mapper.Map<Customer>(model);
			entity = await _unitOfWork.CustomerRepository.UpdateAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<CustomerModel>(entity);
		}
	}
	public partial class CustomerTypeService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CustomerTypeService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public virtual async Task<IEnumerable<CustomerTypeModel>> GetAllAsync()
		{
			var entities = await _unitOfWork.CustomerTypeRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<CustomerTypeModel>>(entities);
		}

		public virtual async Task<CustomerTypeModel> GetByIdAsync(int id)
		{
			var entity = await _unitOfWork.CustomerTypeRepository.GetByIdAsync(id);
			return _mapper.Map<CustomerTypeModel>(entity);
		}

		public virtual async Task DeleteAsync(int id)
		{
			await _unitOfWork.CustomerTypeRepository.DeleteAsync(id);
			await _unitOfWork.SaveChangesAsync();
		}
		public virtual async Task<CustomerTypeModel> AddAsync(CustomerTypeModel model)
		{
			var entity = _mapper.Map<CustomerType>(model);
			entity = await _unitOfWork.CustomerTypeRepository.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<CustomerTypeModel>(entity);
		}

		public virtual async Task<CustomerTypeModel> UpdateAsync(CustomerTypeModel model)
		{
			var entity = _mapper.Map<CustomerType>(model);
			entity = await _unitOfWork.CustomerTypeRepository.UpdateAsync(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<CustomerTypeModel>(entity);
		}
	}
}