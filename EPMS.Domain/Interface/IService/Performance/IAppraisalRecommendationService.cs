using EPMS.Shared.DTOs.FormDTOs;
using EPMS.Shared.DTOs.Common;

namespace EPMS.Domain.Interface.IService.Performance;

public interface IAppraisalRecommendationService
{
    Task<SuccessResponse> CreateAsync(CreateAppraisalRecommendationDto dto);
    Task<SuccessResponse> UpdateAsync(long id, UpdateAppraisalRecommendationDto dto);
    Task<SuccessResponse> DeleteAsync(long id);
    Task<SuccessResponse> GetByIdAsync(long id);
    Task<SuccessResponse> GetAllAsync();
    Task<SuccessResponse> GetByAppraisalIdAsync(long appraisalId);
    Task<SuccessResponse> ApproveAsync(long id, long hrAdminId, string? comments);
    Task<SuccessResponse> RejectAsync(long id, long hrAdminId, string reason);
}
