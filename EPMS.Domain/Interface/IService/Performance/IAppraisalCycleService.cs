using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.AppraisalCycleDTOs;

namespace EPMS.Domain.Interface.IService.Performance
{
    public interface IAppraisalCycleService
    {
        Task<SuccessResponse<IEnumerable<AppraisalCycleDto>>> GetAllAsync();
        Task<SuccessResponse<IEnumerable<AppraisalCycleDto>>> GetActiveCyclesAsync();
        Task<SuccessResponse<AppraisalCycleDto>> GetByIdAsync(long id);
        Task<SuccessResponse<long>> CreateAsync(CreateAppraisalCycleDto dto);
        Task<SuccessResponse> UpdateAsync(long id, UpdateAppraisalCycleDto dto);
        Task<SuccessResponse> DeleteAsync(long id);
        Task<SuccessResponse> LockCycleAsync(long id);
        Task<SuccessResponse> DeactivateAsync(long id);
    }
}