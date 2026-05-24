using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.FormQuestionDTOs;
using Refit;

namespace EPMS.Client.Services.Performance;

public interface IFormQuestionApiClient
{
    [Get("/api/performance/form-questions/template/{templateId}")]
    Task<SuccessResponse<IEnumerable<FormQuestionDto>>> GetByTemplateIdAsync(long templateId);

    [Get("/api/performance/form-questions/{id}")]
    Task<SuccessResponse<FormQuestionDto>> GetByIdAsync(long id);

    [Post("/api/performance/form-questions")]
    Task<SuccessResponse> CreateAsync([Body] CreateFormQuestionDto dto);

    [Put("/api/performance/form-questions/{id}")]
    Task<SuccessResponse> UpdateAsync(long id, [Body] UpdateFormQuestionDto dto);

    [Delete("/api/performance/form-questions/{id}")]
    Task<SuccessResponse> DeleteAsync(long id);
}
