using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.FormTemplateDTOs;
using Refit;

namespace EPMS.Client.Services.Performance;

public interface IFormTemplateApiClient
{
    [Get("/api/performance/form-templates")]
    Task<SuccessResponse<IEnumerable<FormTemplateDto>>> GetAllAsync();

    [Get("/api/performance/form-templates/active")]
    Task<SuccessResponse<IEnumerable<FormTemplateDto>>> GetActiveAsync();

    [Get("/api/performance/form-templates/{id}")]
    Task<SuccessResponse<FormTemplateDto>> GetByIdAsync(long id);

    [Post("/api/performance/form-templates")]
    Task<SuccessResponse<long>> CreateAsync([Body] CreateFormTemplateDto dto);

    [Put("/api/performance/form-templates/{id}")]
    Task<SuccessResponse> UpdateAsync(long id, [Body] UpdateFormTemplateDto dto);

    [Delete("/api/performance/form-templates/{id}")]
    Task<SuccessResponse> DeleteAsync(long id);

    [Post("/api/performance/form-templates/{id}/deactivate")]
    Task<SuccessResponse> DeactivateAsync(long id);

    [Post("/api/performance/form-templates/{id}/reactivate")]
    Task<SuccessResponse> ReactivateAsync(long id);
}
