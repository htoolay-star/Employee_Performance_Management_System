using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Shared;
using EPMS.Domain.Interface.IService.App;
using EPMS.Domain.Interface.IService.Shared;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.CategoryDTOs;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.Enums;
using Mapster;
using static EPMS.Shared.Constants.ServiceResponseMessages;
namespace EPMS.Domain.Services.Shared;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public CategoryService(IUnitOfWork unitOfWork, ICacheService cacheService)
    {
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }
    public async Task<SuccessResponse<IEnumerable<LookUpDto>>> GetLookupAsync()
    {
        var dtos = await _cacheService.GetOrCreateAsync(
            CacheKeys.Shared.CategoryLookups(),
            async () => await _unitOfWork.Shared.Categories.GetLookupAsync(),
            TimeSpan.FromHours(12)
        );

        return SuccessResponse<IEnumerable<LookUpDto>>.Ok(dtos ?? [], CategoryMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<IEnumerable<CategoryDto>>> GetAllCategoriesAsync()
    {
        var categories = await _unitOfWork.Shared.Categories.GetAllAsync();

        var dtos = categories.Select(x => new CategoryDto
        {
            Id = x.Id,
            Code = x.Code,
            Name = x.Name,
            Description = x.Description,
            ParentId = x.ParentId,
            IsActive = x.IsActive,
            ParentName = categories.FirstOrDefault(p => p.Id == x.ParentId)?.Name
        });
        return SuccessResponse<IEnumerable<CategoryDto>>.Ok(dtos, CategoryMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<CategoryDto>> GetCategoryByIdAsync(long id)
    {
        var category = await _unitOfWork.Shared.Categories.GetByIdAsync(id);

        if (category == null)
            return SuccessResponse<CategoryDto>.Fail(CategoryMsg.NotFound(id), ErrorType.NotFound);

        var dto = category.Adapt<CategoryDto>();
        return SuccessResponse<CategoryDto>.Ok(dto, CategoryMsg.Retrieved);
    }

    public async Task<SuccessResponse<long>> CreateCategoryAsync(CreateCategoryDto dto)
    {
        var normalizedCode = dto.Code.Trim().ToUpperInvariant();

        if (await _unitOfWork.Shared.Categories.ExistsByCodeAsync(normalizedCode))
            return SuccessResponse<long>.Fail($"Category with code '{normalizedCode}' already exists.", ErrorType.Conflict);

        if (await _unitOfWork.Shared.Categories.ExistsByNameAsync(dto.Name))
            return SuccessResponse<long>.Fail($"Category with name '{dto.Name}' already exists.", ErrorType.Conflict);

        // Validate parent exists if specified
        if (dto.ParentId.HasValue)
        {
            var parent = await _unitOfWork.Shared.Categories.GetByIdAsync(dto.ParentId.Value);
            if (parent == null)
                return SuccessResponse<long>.Fail($"Parent category with ID '{dto.ParentId.Value}' was not found.", ErrorType.NotFound);
        }

        var category = new Category(dto.Code, dto.Name, dto.Description, dto.ParentId);
        _unitOfWork.Shared.Categories.Add(category);
        await _unitOfWork.CompleteAsync();
        await _cacheService.RemoveAsync(CacheKeys.Shared.CategoryLookups());
        return SuccessResponse<long>.Ok(category.Id, CategoryMsg.Created);
    }

    public async Task<SuccessResponse> UpdateCategoryAsync(long id, UpdateCategoryDto dto)
    {
        var category = await _unitOfWork.Shared.Categories.GetByIdAsync(id);

        if (category == null)
            return SuccessResponse.Fail(CategoryMsg.NotFound(id), ErrorType.NotFound);

        // Check for duplicate name
        if (category.Name != dto.Name && await _unitOfWork.Shared.Categories.ExistsByNameAsync(dto.Name, id))
            return SuccessResponse.Fail($"Another category with name '{dto.Name}' already exists.", ErrorType.Conflict);

        // Validate new parent if changing
        if (category.ParentId != dto.ParentId)
        {
            if (dto.ParentId.HasValue)
            {
                if (dto.ParentId.Value == id)
                    return SuccessResponse.Fail(CategoryMsg.SelfParent, ErrorType.Validation);

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
        await _cacheService.RemoveAsync(CacheKeys.Shared.CategoryLookups());
        return SuccessResponse.Ok(CategoryMsg.Updated);
    }

    public async Task<SuccessResponse> DeleteCategoryAsync(long id)
    {
        var category = await _unitOfWork.Shared.Categories.GetByIdAsync(id);

        if (category == null)
            return SuccessResponse.Fail(CategoryMsg.NotFound(id), ErrorType.NotFound);

        // Check for subcategories
        if (await _unitOfWork.Shared.Categories.HasSubCategoriesAsync(id))
            return SuccessResponse.Fail($"Cannot delete category '{id}' because it has subcategories. Please delete or reassign subcategories first.", ErrorType.Conflict);

        _unitOfWork.Shared.Categories.Delete(category);
        await _unitOfWork.CompleteAsync();
        await _cacheService.RemoveAsync(CacheKeys.Shared.CategoryLookups());
        return SuccessResponse.Ok(CategoryMsg.Deleted);
    }
    public async Task<SuccessResponse> RestoreCategoryAsync(long id)
    {
        var entity = await _unitOfWork.Shared.Categories.GetByIdDeletedAsync(id);
        if (entity == null)
            return SuccessResponse.Fail(CategoryMsg.NotFound(id), ErrorType.NotFound);
        if (!entity.IsDeleted)
            return SuccessResponse.Fail("Item is not deleted.", ErrorType.Validation);
        entity.IsDeleted = false;
        entity.DeletedAt = null;
        entity.DeletedBy = null;
        _unitOfWork.Shared.Categories.Update(entity);
        await _unitOfWork.CompleteAsync();
        await _cacheService.RemoveAsync(CacheKeys.Shared.CategoryLookups());
        return SuccessResponse.Ok(CategoryMsg.Updated);
    }

}