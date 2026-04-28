using EPMS.Shared.PositionDTOs;

namespace EPMS.Application.Services.PositionService
{
    public interface IPositionService
    {
        Task<IEnumerable<PositionResponseDto>> GetAllAsync();
        Task<PositionResponseDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreatePositionDto dto);
        Task<bool> UpdateAsync(UpdatePositionDto dto);
        Task<bool> DeleteAsync(int id);
    }
}