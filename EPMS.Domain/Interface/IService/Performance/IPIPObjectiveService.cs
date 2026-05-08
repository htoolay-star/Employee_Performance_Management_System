using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.PIPObjectiveDTOs;

namespace EPMS.Domain.Interface.IService.Performance;

public interface IPIPObjectiveService
{
    Task<SuccessResponse<IEnumerable<PIPObjectiveDto>>> GetAllAsync();
    Task<SuccessResponse<PIPObjectiveDto>> GetByIdAsync(long id);
    Task<SuccessResponse<IEnumerable<PIPObjectiveDto>>> GetByPIPIdAsync(long pipId);
    Task<SuccessResponse<long>> CreateAsync(CreatePIPObjectiveDto dto);
    Task<SuccessResponse> UpdateAsync(long id, UpdatePIPObjectiveDto dto);
    Task<SuccessResponse> DeleteAsync(long id);
}