using EPMS.Shared.PositionDTOs;


namespace EPMS.Domain.Interface.Irepo.IPositionRepository
{
    public interface IPositionRepository
    {
        Task<IEnumerable<PositionResponseDto>> GetAllAsync();
        Task<PositionResponseDto?> GetByIdAsync(int id);
        Task<(bool Success, string Message, int? Id)> CreateAsync(CreatePositionDto dto);
        Task<(bool Success, string Message)> UpdateAsync(int id, UpdatePositionDto dto);
        Task<(bool Success, string Message)> DeleteAsync(int id);
    }
}
