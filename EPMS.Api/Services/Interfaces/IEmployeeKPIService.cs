using EPMS.Shared.DTOs.KPI;

namespace EPMS.Api.Services.Interfaces
{
    public interface IEmployeeKPIService
    {
        Task SubmitAsync(SubmitEmployeeKPIDto dto);

        Task<List<EmployeeKPIResultDto>> GetEmployeeResultsAsync(
            long employeeId,
            long cycleId);
    }
}
