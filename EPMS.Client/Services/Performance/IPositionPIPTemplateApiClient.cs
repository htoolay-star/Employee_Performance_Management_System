using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.PositionPIPTemplateDTOs;
using Refit;

namespace EPMS.Client.Services.Performance;

public interface IPositionPIPTemplateApiClient
{
    [Get("/api/PositionPIPTemplates")]
    Task<SuccessResponse<IEnumerable<PositionPIPTemplateDto>>> GetAllAsync();

    [Get("/api/PositionPIPTemplates/{id}")]
    Task<SuccessResponse<PositionPIPTemplateDto>> GetByIdAsync(long id);

    [Get("/api/PositionPIPTemplates/position/{positionId}")]
    Task<SuccessResponse<IEnumerable<PositionPIPTemplateDto>>> GetByPositionIdAsync(long positionId);

    [Get("/api/PositionPIPTemplates/position/{positionId}/active")]
    Task<SuccessResponse<IEnumerable<PositionPIPTemplateDto>>> GetActiveByPositionIdAsync(long positionId);

    [Post("/api/PositionPIPTemplates")]
    Task<SuccessResponse<long>> CreateAsync([Body] CreatePositionPIPTemplateDto dto);

    [Put("/api/PositionPIPTemplates/{id}")]
    Task<SuccessResponse> UpdateAsync(long id, [Body] UpdatePositionPIPTemplateDto dto);

    [Delete("/api/PositionPIPTemplates/{id}")]
    Task<SuccessResponse> DeleteAsync(long id);
}
