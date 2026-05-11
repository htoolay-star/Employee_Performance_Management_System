using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;

namespace EPMS.Domain.Interface.IService.Info;

public interface IEmployeeSalaryHistoryService
{
    Task<SuccessResponse<IEnumerable<EmployeeSalaryHistoryDto>>> GetAllAsync();
    Task<SuccessResponse<EmployeeSalaryHistoryDto>> GetByIdAsync(long id);
    Task<SuccessResponse<IEnumerable<EmployeeSalaryHistoryDto>>> GetByEmployeeIdAsync(long employeeId);
    // No Create, Update or Delete - history is auto-generated append-only
}
