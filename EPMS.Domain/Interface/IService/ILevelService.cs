using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.LevelDTOs;

namespace EPMS.Domain.Interfaces;

public interface ILevelService
{
    Task<SuccessResponse<IEnumerable<LevelLookupDto>>> GetLookupAsync();
    Task<SuccessResponse<IEnumerable<LevelDto>>> GetAllAsync();
    Task<SuccessResponse<LevelDto>> GetByIdAsync(long id);
    Task<SuccessResponse<long>> CreateAsync(CreateLevelDto dto);
    Task<SuccessResponse> UpdateAsync(long id, UpdateLevelDto dto);
    Task<SuccessResponse> DeleteAsync(long id);
}
