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

    [Get("/api/performance/appraisals/employee/{employeeId}")]
    Task<SuccessResponse<IEnumerable<AppraisalDto>>> GetByEmployeeIdAsync(long employeeId);

    [Post("/api/performance/appraisals")]
    Task<SuccessResponse> CreateAsync([Body] CreateAppraisalDto dto);

    [Post("/api/performance/appraisals/submit")]
    Task<SuccessResponse> SubmitAsync([Body] AppraisalSubmissionDto dto);

    [Delete("/api/performance/appraisals/{id}")]
    Task<SuccessResponse> DeleteAsync(long id);

    [Put("/api/performance/appraisals/{id}/lock")]
    Task<SuccessResponse> LockAsync(long id, [Body] UnlockRequestDto request);

    [Put("/api/performance/appraisals/{id}/unlock")]
    Task<SuccessResponse> UnlockAsync(long id, [Body] UnlockRequestDto request);
}
