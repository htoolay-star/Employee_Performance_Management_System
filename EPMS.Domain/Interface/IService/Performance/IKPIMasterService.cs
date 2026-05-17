using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.KPIMasterDTOs;

namespace EPMS.Domain.Interface.IService.Performance
{
    public interface IKPIMasterService
    {
        Task<SuccessResponse<IEnumerable<LookUpDto>>> GetLookupAsync();
        Task<SuccessResponse<IEnumerable<KPIMasterDto>>> GetAllAsync();
        Task<SuccessResponse<IEnumerable<KPIMasterDto>>> GetActiveAsync();
        Task<SuccessResponse<KPIMasterDto>> GetByIdAsync(long id);
        Task<SuccessResponse<long>> CreateAsync(CreateKPIMasterDto dto);
        Task<SuccessResponse> UpdateAsync(long id, UpdateKPIMasterDto dto);
        Task<SuccessResponse> DeleteAsync(long id);
        Task<SuccessResponse> DeactivateAsync(long id);
        Task<SuccessResponse> ReactivateAsync(long id);
    }
}