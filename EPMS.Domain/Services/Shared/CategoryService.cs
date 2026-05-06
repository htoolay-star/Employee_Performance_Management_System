using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Shared;
using EPMS.Domain.Interface.Irepo.Shared;
using EPMS.Domain.Interface.IService.Shared;
using EPMS.Shared.DTOs.CategoryDTOs;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.Enums;

namespace EPMS.Domain.Services.Shared;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SuccessResponse<IEnumerable<CategoryDto>>> GetAllCategoriesAsync()
    {
        var categories = await _unitOfWork.Shared.Categories.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
        return SuccessResponse<IEnumerable<CategoryDto>>.Ok(dtos, "Categories retrieved successfully.");
    }

    public async Task<SuccessResponse<CategoryDto>> GetCategoryByIdAsync(int id)
    {
        var category = await _unitOfWork.Shared.Categories.GetByIdAsync(id);

        if (category == null)
            return SuccessResponse<CategoryDto>.Fail($"Category with ID '{id}' was not found.", ErrorType.NotFound);

        var dto = _mapper.Map<CategoryDto>(category);
        return SuccessResponse<CategoryDto>.Ok(dto, "Category retrieved successfully.");
    }

    public async Task<SuccessResponse<long>> CreateCategoryAsync(CreateCategoryDto dto)
    {
        var normalizedModule = dto.Module.Trim().ToUpperInvariant();
        var normalizedCode = dto.Code.Trim().ToUpperInvariant();

        if (await _unitOfWork.Shared.Categories.ExistsByCodeAsync(normalizedCode, normalizedModule))
            return SuccessResponse<long>.Fail($"Category with code '{normalizedCode}' already exists in module '{normalizedModule}'.", ErrorType.Conflict);

        if (await _unitOfWork.Shared.Categories.ExistsByNameAsync(dto.Name, normalizedModule))
            return SuccessResponse<long>.Fail($"Category with name '{dto.Name}' already exists in module '{normalizedModule}'.", ErrorType.Conflict);

        // Validate parent exists if specified
        if (dto.ParentId.HasValue)
        {
            var parent = await _unitOfWork.Shared.Categories.GetByIdAsync(dto.ParentId.Value);
            if (parent == null)
                return SuccessResponse<long>.Fail($"Parent category with ID '{dto.ParentId.Value}' was not found.", ErrorType.NotFound);
        }

        var category = new Category(dto.Module, dto.Code, dto.Name, dto.Description, dto.ParentId);
        _unitOfWork.Shared.Categories.Add(category);
        await _unitOfWork.CompleteAsync();
        return SuccessResponse<long>.Ok(category.Id, "Category created successfully.");
    }

    public async Task<SuccessResponse> UpdateCategoryAsync(int id, UpdateCategoryDto dto)
    {
        var category = await _unitOfWork.Shared.Categories.GetByIdAsync(id);

        if (category == null)
            return SuccessResponse.Fail($"Category with ID '{id}' was not found.", ErrorType.NotFound);

        var normalizedModule = category.Module;

        // Check for duplicate name
        if (category.Name != dto.Name && await _unitOfWork.Shared.Categories.ExistsByNameAsync(dto.Name, normalizedModule, id))
            return SuccessResponse.Fail($"Another category with name '{dto.Name}' already exists in module '{normalizedModule}'.", ErrorType.Conflict);

        // Validate new parent if changing
        if (category.ParentId != dto.ParentId)
        {
            if (dto.ParentId.HasValue)
            {
                if (dto.ParentId.Value == id)
                    return SuccessResponse.Fail("A category cannot be its own parent.", ErrorType.Validation);

                var parent = await _unitOfWork.Shared.Categories.GetByIdAsync(dto.ParentId.Value);
                if (parent == null)
                    return SuccessResponse.Fail($"Parent category with ID '{dto.ParentId.Value}' was not found.", ErrorType.NotFound);
            }
            category.MoveToParent(dto.ParentId);
        }

        category.UpdateDetails(dto.Name, dto.Description);

        if (dto.IsActive) category.Reactivate();
        else category.Deactivate();

        await _unitOfWork.CompleteAsync();
        return SuccessResponse.Ok("Category updated successfully.");
    }

    public async Task<SuccessResponse> DeleteCategoryAsync(int id)
    {
        var category = await _unitOfWork.Shared.Categories.GetByIdAsync(id);

        if (category == null)
            return SuccessResponse.Fail($"Category with ID '{id}' was not found.", ErrorType.NotFound);

        // Check for subcategories
        if (await _unitOfWork.Shared.Categories.HasSubCategoriesAsync(id))
            return SuccessResponse.Fail($"Cannot delete category '{id}' because it has subcategories. Please delete or reassign subcategories first.", ErrorType.Conflict);

        _unitOfWork.Shared.Categories.Delete(category);
        await _unitOfWork.CompleteAsync();
        return SuccessResponse.Ok("Category deleted successfully.");
    }
}
