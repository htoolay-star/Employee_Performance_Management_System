using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.PIPDTOs;

namespace EPMS.Domain.Interface.IService.Performance
{
    public interface IPIPService
    {
        Task<SuccessResponse<IEnumerable<PIPDto>>> GetAllAsync();
        Task<SuccessResponse<IEnumerable<PIPDto>>> GetActivePIPsAsync();
        Task<SuccessResponse<IEnumerable<PIPDto>>> GetByEmployeeIdAsync(long employeeId);
        Task<SuccessResponse<IEnumerable<PIPDto>>> GetByManagerIdAsync(long managerId);
        Task<SuccessResponse<IEnumerable<PIPDto>>> GetMyPIPsAsync();
        Task<SuccessResponse<PIPDto>> GetByIdAsync(long id);
        Task<SuccessResponse<long>> CreateAsync(CreatePIPDto dto);
        Task<SuccessResponse> UpdateAsync(long id, UpdatePIPDto dto);
        Task<SuccessResponse> DeleteAsync(long id);
        Task<SuccessResponse> ConcludeAsync(long id, bool isSuccessful, string? notes);
        Task<SuccessResponse> ExtendAsync(long id, DateOnly newEndDate, string reason);
    }
}