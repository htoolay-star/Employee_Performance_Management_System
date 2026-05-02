using EPMS.Shared.DTOs.PositionDTOs;

namespace EPMS.Domain.Interfaces;

public interface IPositionService
{
    Task<IEnumerable<PositionDto>> GetAllAsync();
    Task<PositionDto?> GetByIdAsync(long id);
    Task<long> CreateAsync(CreatePositionDto dto);
    Task UpdateAsync(long id, UpdatePositionDto dto);
    Task DeleteAsync(long id);
}
