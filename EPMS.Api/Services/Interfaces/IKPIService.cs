using EPMS.Shared.DTOs.KPI;

namespace EPMS.Api.Services.Interfaces
{
    public interface IKPIService
    {
        Task<List<KPIMasterDto>> GetAllAsync();

        Task CreateAsync(CreateKPIMasterDto dto);
    }
}
