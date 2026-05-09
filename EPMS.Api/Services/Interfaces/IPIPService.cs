using EPMS.Shared.DTOs.PerformanceImprovementPlan;

namespace EPMS.Api.Services.Interfaces
{
    public interface IPIPService
    {
        Task<PIPDto> CreateAsync(CreatePIPDto dto);
        Task AddProgressAsync(AddPIPProgressDto dto);
        Task<List<PIPDto>> GetAllAsync();
    }
}
