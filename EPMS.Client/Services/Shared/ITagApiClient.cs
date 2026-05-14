using EPMS.Shared.DTOs.CategoryDTOs;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.TagDTOs;
using Refit;

namespace EPMS.Client.Services.Shared
{
    public interface ITagApiClient
    {
        [Get("/api/tags/lookup")]
        Task<SuccessResponse<IEnumerable<LookUpDto>>> GetLookupAsync();

        [Get("/api/tags")]
        Task<SuccessResponse<IEnumerable<TagDto>>> GetAllAsync();

        [Get("/api/tags/{id}")]
        Task<SuccessResponse<TagDto>> GetByIdAsync(long id);

        [Post("/api/tags")]
        Task<SuccessResponse<long>> CreateAsync(CreateTagDto dto);

        [Put("/api/tags/{id}")]
        Task<SuccessResponse> UpdateAsync(long id, UpdateTagDto dto);

        [Delete("/api/tags/{id}")]
        Task<SuccessResponse> DeleteAsync(long id);
    }
}
