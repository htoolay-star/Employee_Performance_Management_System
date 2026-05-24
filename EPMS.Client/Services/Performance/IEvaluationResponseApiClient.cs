using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.FormDTOs;
using Refit;

namespace EPMS.Client.Services.Performance;

public interface IEvaluationResponseApiClient
{
    [Get("/api/performance/evaluation-responses/appraisal/{appraisalId}/my-responses")]
    Task<SuccessResponse<EvaluationFormFillDto>> GetFormFillAsync(long appraisalId, [Query] long evaluatorId, [Query] string role);

    [Get("/api/performance/evaluation-responses/appraisal/{appraisalId}/self-assessment")]
    Task<SuccessResponse<EvaluationFormFillDto>> GetSelfAssessmentAsync(long appraisalId, [Query] long managerId);

    [Put("/api/performance/evaluation-responses/{id}")]
    Task<SuccessResponse> UpdateAsync(long id, [Body] UpdateEvaluationResponseDto dto);

    [Post("/api/performance/evaluation-responses/submit-role")]
    Task<SuccessResponse> SubmitRoleResponsesAsync([Body] SubmitRoleRequestDto dto);
}
