using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.FormDTOs;

namespace EPMS.Domain.Interface.IService.Performance;

public interface IEvaluationResponseService
{
    Task<SuccessResponse> CreateAsync(CreateEvaluationResponseDto dto);
    Task<SuccessResponse> UpdateAsync(long id, UpdateEvaluationResponseDto dto);
    Task<SuccessResponse> DeleteAsync(long id);
    Task<SuccessResponse> GetByIdAsync(long id);
    Task<SuccessResponse> GetAllAsync();
    Task<SuccessResponse> GetByAppraisalIdAsync(long appraisalId);
    Task<SuccessResponse> GetByTemplateIdAsync(long templateId);
    Task<SuccessResponse> GetByQuestionIdAsync(long questionId);
    Task<SuccessResponse> SubmitRoleResponsesAsync(long appraisalId, string role);
    Task<SuccessResponse> GetFormFillAsync(long appraisalId, string role);
    Task<SuccessResponse> GetSelfAssessmentAsync(long appraisalId);
    Task<SuccessResponse> GetEvaluationViewAsync(long appraisalId, string role, long? evaluatorId = null);
    Task<SuccessResponse> GetMyFormsAsync(string? roleGroup = null);
    Task<SuccessResponse> GetMyAppraisalFormsAsync();
    Task<SuccessResponse> GetPendingEvaluationsAsync();
}