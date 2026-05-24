using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.FormDTOs;
using Refit;

namespace EPMS.Client.Services.Performance;

public interface IAppraisalApiClient
{
    [Get("/api/performance/appraisals")]
    Task<SuccessResponse<IEnumerable<AppraisalDto>>> GetAllAsync();

    [Get("/api/performance/appraisals/{id}")]
    Task<SuccessResponse<AppraisalDto>> GetByIdAsync(long id);

    [Get("/api/performance/appraisals/{id}/fill")]
    Task<SuccessResponse<AppraisalFillDto>> GetFillAsync(long id);

    [Get("/api/performance/appraisals/{id}/view")]
    Task<SuccessResponse<AppraisalFillDto>> GetViewAsync(long id);

    [Get("/api/performance/appraisals/my-kpi")]
    Task<SuccessResponse<IEnumerable<AppraisalFillDto>>> GetMyKpiAsync();

    [Get("/api/performance/appraisals/pending")]
    Task<SuccessResponse<IEnumerable<AppraisalDto>>> GetPendingAsync();

    [Get("/api/performance/appraisals/employee/{employeeId}")]
    Task<SuccessResponse<IEnumerable<AppraisalDto>>> GetByEmployeeIdAsync(long employeeId);

    [Get("/api/performance/appraisals/my-evaluations")]
    Task<SuccessResponse<IEnumerable<AppraisalDto>>> GetMyEvaluationsAsync();

    [Get("/api/performance/appraisals/entity/{entityType}/cycle/{cycleId}")]
    Task<SuccessResponse<IEnumerable<AppraisalDto>>> GetByEntityTypeAndCycleAsync(string entityType, long cycleId);

    [Post("/api/performance/appraisals")]
    Task<SuccessResponse> CreateAsync([Body] CreateAppraisalDto dto);

    [Post("/api/performance/appraisals/submit")]
    Task<SuccessResponse> SubmitAsync([Body] AppraisalSubmissionDto dto);

    [Put("/api/performance/appraisals/{id}/details")]
    Task<SuccessResponse> UpdateDetailActualValuesAsync(long id, [Body] List<AppraisalDetailDto> details);

    [Delete("/api/performance/appraisals/{id}")]
    Task<SuccessResponse> DeleteAsync(long id);

    [Put("/api/performance/appraisals/{id}/lock")]
    Task<SuccessResponse> LockAsync(long id, [Body] UnlockRequestDto request);

    [Put("/api/performance/appraisals/{id}/unlock")]
    Task<SuccessResponse> UnlockAsync(long id, [Body] UnlockRequestDto request);

    [Post("/api/performance/appraisals/generate/{cycleId}")]
    Task<SuccessResponse> GenerateForCycleAsync(long cycleId);

    [Post("/api/performance/appraisals/{id}/finalize")]
    Task<SuccessResponse> FinalizeAsync(long id);

    [Post("/api/performance/appraisals/{id}/finalize-kpi")]
    Task<SuccessResponse> FinalizeKpiAsync(long id);
}
