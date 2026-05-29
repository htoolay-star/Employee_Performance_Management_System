using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.FormDTOs;
using Refit;

namespace EPMS.Client.Services.Performance;

public interface IEvaluationResponseApiClient
{
    [Get("/api/performance/evaluation-responses/appraisal/{appraisalId}/my-responses")]
    Task<SuccessResponse<EvaluationFormFillDto>> GetFormFillAsync(long appraisalId, [Query] string role);

    [Get("/api/performance/evaluation-responses/appraisal/{appraisalId}/self-assessment")]
    Task<SuccessResponse<EvaluationFormFillDto>> GetSelfAssessmentAsync(long appraisalId);

    [Get("/api/performance/evaluation-responses/appraisal/{appraisalId}/view")]
    Task<SuccessResponse<EvaluationFormFillDto>> GetEvaluationViewAsync(long appraisalId, [Query] string role, [Query] long? evaluatorId = null);

    [Put("/api/performance/evaluation-responses/{id}")]
    Task<SuccessResponse> UpdateAsync(long id, [Body] UpdateEvaluationResponseDto dto);

    [Post("/api/performance/evaluation-responses/submit-role")]
    Task<SuccessResponse> SubmitRoleResponsesAsync([Body] SubmitRoleRequestDto dto);

    [Get("/api/performance/evaluation-responses/my-forms")]
    Task<SuccessResponse<IEnumerable<MyEvaluationFormDto>>> GetMyFormsAsync([Query] string? roleGroup = null);

    [Get("/api/performance/evaluation-responses/my-appraisal-forms")]
    Task<SuccessResponse<IEnumerable<MyEvaluationFormDto>>> GetMyAppraisalFormsAsync();

    [Get("/api/performance/evaluation-responses/pending")]
    Task<SuccessResponse<IEnumerable<PendingEvaluationDto>>> GetPendingEvaluationsAsync();
}
