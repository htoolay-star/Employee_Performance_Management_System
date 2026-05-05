using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.LevelDTOs;

namespace EPMS.Domain.Interfaces;

public interface ILevelService
{
    Task<SuccessResponse<IEnumerable<LevelDto>>> GetAllAsync();
    Task<SuccessResponse<LevelDto>> GetByIdAsync(int id);
    Task<SuccessResponse<int>> CreateAsync(CreateLevelDto dto);
    Task<SuccessResponse> UpdateAsync(int id, UpdateLevelDto dto);
    Task<SuccessResponse> DeleteAsync(int id);
}
