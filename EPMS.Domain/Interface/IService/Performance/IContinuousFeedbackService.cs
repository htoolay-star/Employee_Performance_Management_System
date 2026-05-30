using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.ContinuousFeedbackDTOs;

namespace EPMS.Domain.Interface.IService.Performance
{
    public interface IContinuousFeedbackService
    {
        Task<SuccessResponse<IEnumerable<ContinuousFeedbackDto>>> GetReceivedFeedbackAsync();
        Task<SuccessResponse<IEnumerable<ContinuousFeedbackDto>>> GetGivenFeedbackAsync();
        Task<SuccessResponse<ContinuousFeedbackDto>> GetByIdAsync(long id);
        Task<SuccessResponse<IEnumerable<ContinuousFeedbackDto>>> GetByEmployeeIdAsync(long employeeId);
        Task<SuccessResponse<IEnumerable<ContinuousFeedbackDto>>> GetByUserIdAsync(long userId);
        Task<SuccessResponse<long>> CreateAsync(CreateContinuousFeedbackDto dto);
        Task<SuccessResponse> UpdateAsync(long id, UpdateContinuousFeedbackDto dto);
        Task<SuccessResponse> DeleteAsync(long id);
    }
}