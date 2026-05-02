using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Shared;
using EPMS.Domain.Interface.Irepo.Shared;
using EPMS.Domain.Interface.IService.Shared;
using EPMS.Shared.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Services.Shared
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Shared.Categories.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.Shared.Categories.GetByIdAsync(id);
            return category == null ? null : _mapper.Map<CategoryDto>(category);
        }

        public async Task CreateCategoryAsync(CreateCategoryDto dto)
        {
            var category = new Category(dto.Module, dto.Code, dto.Name, dto.Description, dto.ParentId);
            _unitOfWork.Shared.Categories.Add(category);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateCategoryAsync(int id, UpdateCategoryDto dto)
        {
            var category = await _unitOfWork.Shared.Categories.GetByIdAsync(id);
            if (category == null) throw new Exception("Not Found Category");

            category.UpdateDetails(dto.Name, dto.Description);


            if (category.ParentId != dto.ParentId)
            {
                category.MoveToParent(dto.ParentId);
            }

            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Shared.Categories.GetByIdAsync(id);
            if (category != null)
            {
                _unitOfWork.Shared.Categories.Delete(category);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
