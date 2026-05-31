using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.OneOnOneMeetingDTOs;

namespace EPMS.Domain.Interface.IService.Performance
{
    public interface IOneOnOneMeetingService
    {
        Task<SuccessResponse<IEnumerable<OneOnOneMeetingDto>>> GetAllAsync();
        Task<SuccessResponse<IEnumerable<OneOnOneMeetingDto>>> GetUpcomingAsync();
        Task<SuccessResponse<OneOnOneMeetingDto>> GetByIdAsync(long id);
        Task<SuccessResponse<IEnumerable<OneOnOneMeetingDto>>> GetByEmployeeIdAsync(long employeeId);
        Task<SuccessResponse<IEnumerable<OneOnOneMeetingDto>>> GetByManagerIdAsync(long managerId);
        Task<SuccessResponse<long>> CreateAsync(CreateOneOnOneMeetingDto dto);
        Task<SuccessResponse> UpdateAsync(long id, UpdateOneOnOneMeetingDto dto);
        Task<SuccessResponse> DeleteAsync(long id);
        Task<SuccessResponse> CompleteAsync(long id, CompleteMeetingDto dto);
        Task<SuccessResponse> CancelAsync(long id);
        Task<SuccessResponse> AcknowledgeAsync(long id);
        Task<SuccessResponse> ConfirmAsync(long id);
        Task<SuccessResponse> RescheduleByEmployeeAsync(long id, RescheduleMeetingDto dto);
        Task<SuccessResponse> AcceptRescheduleAsync(long id);
        Task<SuccessResponse> RescheduleByManagerAsync(long id, RescheduleMeetingDto dto);
    }
}