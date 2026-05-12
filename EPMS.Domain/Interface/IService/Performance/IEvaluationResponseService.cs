using EPMS.Shared.DTOs.FormDTOs;
using EPMS.Shared.DTOs.Common;

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
}