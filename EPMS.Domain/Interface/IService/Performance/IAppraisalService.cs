using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.FormDTOs;

namespace EPMS.Domain.Interface.IService.Performance;

public interface IAppraisalService
{
    Task<SuccessResponse> CreateAsync(CreateAppraisalDto dto);
    Task<SuccessResponse> UpdateAsync(long id, UpdateAppraisalDto dto);
    Task<SuccessResponse> DeleteAsync(long id);
    Task<SuccessResponse> GetByIdAsync(long id);
    Task<SuccessResponse> GetAllAsync();
    Task<SuccessResponse> GetByEmployeeIdAsync(long employeeId);
    Task<SuccessResponse> GetMyEvaluationsAsync();
    Task<SuccessResponse> GetByEntityTypeAndCycleAsync(string entityType, long cycleId);
    Task<SuccessResponse> GetByEntityTypeAsync(string entityType);
    Task<SuccessResponse> UpdateDetailActualValuesAsync(long appraisalId, List<AppraisalDetailDto> details);
    Task<SuccessResponse> GetAppraisalFillAsync(long id);
    Task<SuccessResponse> GetAppraisalViewAsync(long id);
    Task<SuccessResponse> GetMyKpiAsync();
    Task<SuccessResponse> GetPendingAsync();
    Task<SuccessResponse> SubmitAsync(AppraisalSubmissionDto dto);

    Task AutoGenerateForCycleAsync(long cycleId);
    Task<SuccessResponse> FinalizeKpiAsync(long id);
    Task<SuccessResponse> FinalizeEvaluationAsync(long appraisalId, string role);
    Task<SuccessResponse> UnlockRoleAsync(long id, string role);
    Task<SuccessResponse> RequestKpiUnlockAsync(long id);
    Task<SuccessResponse> DeclineKpiUnlockAsync(long id);
    Task<SuccessResponse> GetManagerSelfPendingAsync();
    Task<SuccessResponse> ApproveSelfAssessmentAsync(long appraisalId);
    Task AutoFinalizeAndCalculateScoreAsync(long appraisalId);
    Task<SuccessResponse> GetEmployeeFormsOverviewAsync(long appraisalId);
    Task<SuccessResponse> GetMy360FeedbackAsync(long appraisalId);
}