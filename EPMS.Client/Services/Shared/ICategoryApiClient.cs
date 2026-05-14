using EPMS.Shared.DTOs.CategoryDTOs;
using EPMS.Shared.DTOs.Common;
using Refit;

namespace EPMS.Client.Services.Shared
{
    public interface ICategoryApiClient
    {
        [Get("/api/categories/lookup")]
        Task<SuccessResponse<IEnumerable<LookUpDto>>> GetLookupAsync();

        [Get("/api/categories")]
        Task<SuccessResponse<IEnumerable<CategoryDto>>> GetAllAsync();

        [Get("/api/categories/{id}")]
        Task<SuccessResponse<CategoryDto>> GetByIdAsync(long id);

        [Post("/api/categories")]
        Task<SuccessResponse<long>> CreateAsync([Body] CreateCategoryDto dto);

        [Put("/api/categories/{id}")]
        Task<SuccessResponse> UpdateAsync(long id, [Body] UpdateCategoryDto dto);

        [Delete("/api/categories/{id}")]
        Task<SuccessResponse> DeleteAsync(long id);
    }
}