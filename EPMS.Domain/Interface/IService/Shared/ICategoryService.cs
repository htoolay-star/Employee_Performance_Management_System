using EPMS.Shared.DTOs.CategoryDTOs;
using EPMS.Shared.DTOs.Common;

namespace EPMS.Domain.Interface.IService.Shared;

public interface ICategoryService
{
    Task<SuccessResponse<IEnumerable<LookUpDto>>> GetLookupAsync();
    Task<SuccessResponse<IEnumerable<CategoryDto>>> GetAllCategoriesAsync();
    Task<SuccessResponse<CategoryDto>> GetCategoryByIdAsync(long id);
    Task<SuccessResponse<long>> CreateCategoryAsync(CreateCategoryDto dto);
    Task<SuccessResponse> UpdateCategoryAsync(long id, UpdateCategoryDto dto);
    Task<SuccessResponse> DeleteCategoryAsync(long id);
}
