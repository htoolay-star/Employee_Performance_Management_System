using EPMS.Shared.DTOs.LevelDTOs;

namespace EPMS.Domain.Interfaces;

public interface ILevelService
{
    Task<IEnumerable<LevelDto>> GetAllAsync();
    Task<LevelDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateLevelDto dto);
    Task UpdateAsync(int id, UpdateLevelDto dto);
    Task DeleteAsync(int id);
}
