using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PositionDTOs;
using EPMS.Shared.Features.Positions;
using Refit;

namespace EPMS.Client.Services;

public interface IPositionApiClient
{
    [Get("/api/positions/lookup")]
    Task<SuccessResponse<IEnumerable<LookUpDto>>> GetLookupAsync();

    [Get("/api/positions")]
    Task<SuccessResponse<IEnumerable<PositionDto>>> GetAllAsync();

    [Get("/api/positions/paged")]
    Task<SuccessResponse<PaginatedResponse<PositionGridItemDto>>> GetPagedAsync([Query] PositionQueryParameters parameters);

    [Get("/api/positions/{id}")]
    Task<SuccessResponse<PositionDto>> GetByIdAsync(long id);

    [Post("/api/positions")]
    Task<SuccessResponse<long>> CreateAsync([Body] CreatePositionDto dto);

    [Put("/api/positions/{id}")]
    Task<SuccessResponse> UpdateAsync(long id, [Body] UpdatePositionDto dto);

    [Delete("/api/positions/{id}")]
    Task<SuccessResponse> DeleteAsync(long id);
}