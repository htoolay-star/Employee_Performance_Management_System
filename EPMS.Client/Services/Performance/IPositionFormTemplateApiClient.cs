using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.PositionFormTemplateDTOs;
using Refit;

namespace EPMS.Client.Services.Performance;

public interface IPositionFormTemplateApiClient
{
    [Get("/api/PositionFormTemplates")]
    Task<SuccessResponse<IEnumerable<PositionFormTemplateDto>>> GetAllAsync();

    [Get("/api/PositionFormTemplates/{id}")]
    Task<SuccessResponse<PositionFormTemplateDto>> GetByIdAsync(long id);

    [Get("/api/PositionFormTemplates/position/{positionId}")]
    Task<SuccessResponse<IEnumerable<PositionFormTemplateDto>>> GetByPositionIdAsync(long positionId);

    [Post("/api/PositionFormTemplates")]
    Task<SuccessResponse<long>> CreateAsync([Body] CreatePositionFormTemplateDto dto);

    [Put("/api/PositionFormTemplates/{id}")]
    Task<SuccessResponse> UpdateAsync(long id, [Body] UpdatePositionFormTemplateDto dto);

    [Delete("/api/PositionFormTemplates/{id}")]
    Task<SuccessResponse> DeleteAsync(long id);
}
