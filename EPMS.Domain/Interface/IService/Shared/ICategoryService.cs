using EPMS.Shared.DTOs.CategoryDTOs;
using EPMS.Shared.DTOs.Common;

namespace EPMS.Domain.Interface.IService.Shared;

public interface ICategoryService
{
    Task<SuccessResponse<IEnumerable<CategoryDto>>> GetAllCategoriesAsync();
    Task<SuccessResponse<CategoryDto>> GetCategoryByIdAsync(int id);
    Task<SuccessResponse<long>> CreateCategoryAsync(CreateCategoryDto dto);
    Task<SuccessResponse> UpdateCategoryAsync(int id, UpdateCategoryDto dto);
    Task<SuccessResponse> DeleteCategoryAsync(int id);
}
