using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.LevelDTOs;
using Refit;

namespace EPMS.Client.Services.Hr;

public interface ILevelApiClient
{
    [Get("/api/levels/lookup")]
    Task<SuccessResponse<IEnumerable<LookUpDto>>> GetLookupAsync();

    [Get("/api/levels")]
    Task<SuccessResponse<IEnumerable<LevelDto>>> GetAllAsync();

    [Get("/api/levels/{id}")]
    Task<SuccessResponse<LevelDto>> GetByIdAsync(long id);

    [Post("/api/levels")]
    Task<SuccessResponse<long>> CreateAsync([Body] CreateLevelDto dto);

    [Put("/api/levels/{id}")]
    Task<SuccessResponse> UpdateAsync(long id, [Body] UpdateLevelDto dto);

    [Delete("/api/levels/{id}")]
    Task<SuccessResponse> DeleteAsync(long id);
}