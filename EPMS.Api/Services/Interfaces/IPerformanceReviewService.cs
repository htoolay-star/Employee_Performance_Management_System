using EPMS.Shared.DTOs;

namespace EPMS.Api.Services.Interfaces
{
    public interface IPerformanceReviewService
    {
        Task<PerformanceReviewDto> CreateAsync(CreatePerformanceReivewDto dto);
        Task<List<PerformanceReviewDto>> GetAllAsync();
        Task<PerformanceReviewDto?> GetByIdAsync(Guid id);
    }
}
