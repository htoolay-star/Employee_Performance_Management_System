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

    [Get("/api/performance/appraisals/entity/{entityType}")]
    Task<SuccessResponse<IEnumerable<AppraisalDto>>> GetByEntityTypeAsync(string entityType);

    [Post("/api/performance/appraisals")]
    Task<SuccessResponse> CreateAsync([Body] CreateAppraisalDto dto);

    [Post("/api/performance/appraisals/submit")]
    Task<SuccessResponse> SubmitAsync([Body] AppraisalSubmissionDto dto);

    [Put("/api/performance/appraisals/{id}/details")]
    Task<SuccessResponse> UpdateDetailActualValuesAsync(long id, [Body] List<AppraisalDetailDto> details);

    [Delete("/api/performance/appraisals/{id}")]
    Task<SuccessResponse> DeleteAsync(long id);

    [Post("/api/performance/appraisals/{id}/finalize")]
    Task<SuccessResponse> FinalizeAsync(long id);

    [Post("/api/performance/appraisals/{id}/finalize-kpi")]
    Task<SuccessResponse> FinalizeKpiAsync(long id);

    [Post("/api/performance/appraisals/{id}/finalize-evaluation")]
    Task<SuccessResponse> FinalizeEvaluationAsync(long id, [Query] string role);

    [Get("/api/performance/appraisals/{appraisalId}/forms")]
    Task<SuccessResponse<EmployeeFormsOverviewDto>> GetEmployeeFormsAsync(long appraisalId);

    [Get("/api/performance/appraisals/{appraisalId}/my-360-feedback")]
    Task<SuccessResponse<EmployeeFormsOverviewDto>> GetMy360FeedbackAsync(long appraisalId);

    [Get("/api/performance/appraisals/manager-self-pending")]
    Task<SuccessResponse<IEnumerable<AppraisalDto>>> GetManagerSelfPendingAsync();

    [Put("/api/performance/appraisals/{id}/approve-self")]
    Task<SuccessResponse> ApproveSelfAssessmentAsync(long id);
}
