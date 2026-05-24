using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.KPIWeightPriorityDTOs;

namespace EPMS.Domain.Interface.IService.Performance;

public interface IKPIWeightPriorityService
{
    Task<SuccessResponse<IEnumerable<KPIWeightPriorityDto>>> GetAllAsync();
    Task<SuccessResponse<IEnumerable<KPIWeightPriorityDto>>> GetActiveAsync();
    Task<SuccessResponse<KPIWeightPriorityDto>> GetByIdAsync(long id);
    Task<SuccessResponse<KPIWeightPriorityDto>> GetByLevelNameAsync(string levelName);
    Task<SuccessResponse<long>> CreateAsync(CreateKPIWeightPriorityDto dto);
    Task<SuccessResponse> UpdateAsync(long id, UpdateKPIWeightPriorityDto dto);
    Task<SuccessResponse> RestoreAsync(long id);
    Task<SuccessResponse> DeleteAsync(long id);
}
