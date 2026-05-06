using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;

namespace EPMS.Domain.Interface.IService.Info;

public interface IEmployeeSalaryHistoryService
{
    Task<SuccessResponse<IEnumerable<EmployeeSalaryHistoryDto>>> GetAllAsync();
    Task<SuccessResponse<EmployeeSalaryHistoryDto>> GetByIdAsync(long id);
    Task<SuccessResponse<IEnumerable<EmployeeSalaryHistoryDto>>> GetByEmployeeIdAsync(long employeeId);
    Task<SuccessResponse<long>> CreateAsync(CreateEmployeeSalaryHistoryDto dto);
    // No Update or Delete - history is append-only
}
