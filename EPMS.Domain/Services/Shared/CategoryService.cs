using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Shared;
using EPMS.Domain.Interface.Irepo.Shared;
using EPMS.Domain.Interface.IService.Shared;
using EPMS.Shared.DTOs.SharedDTOs.CategoryDTOs;

namespace EPMS.Domain.Services.Shared
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepo, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            return category == null ? null : _mapper.Map<CategoryDto>(category);
        }
        public async Task CreateCategoryAsync(CreateCategoryDto dto)
        {
            var isExist = await _categoryRepo.AnyAsync(x => x.Code == dto.Code.ToUpper());
            if (isExist)
            {
                throw new Exception("Category Code already exists.");
            }

            var category = new Category(dto.Module, dto.Code, dto.Name, dto.Description, dto.ParentId);
            _categoryRepo.Add(category);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateCategoryAsync(int id, UpdateCategoryDto dto)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null) throw new Exception("NotFound Category");

            if (dto.ParentId.HasValue && dto.ParentId.Value == id)
            {
                throw new Exception("A category cannot be its own parent.");
            }

            category.UpdateDetails(dto.Name, dto.Description);

            if (category.ParentId != dto.ParentId)
            {
                category.MoveToParent(dto.ParentId);
            }

            _categoryRepo.Update(category);
            await _unitOfWork.CompleteAsync();
        }

        // Task<string> အစား Task လို့ ပြင်လိုက်ပါတယ်
        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null) throw new Exception("NotFound Category");

            category.Deactivate(); // Soft Delete
            _categoryRepo.Update(category);
            await _unitOfWork.CompleteAsync();
        }
    }
}