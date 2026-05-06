using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;

namespace EPMS.Domain.Interface.IService.Info;

public interface IEmployeePayrollInfoService
{
    Task<SuccessResponse<IEnumerable<EmployeePayrollInfoDto>>> GetAllAsync();
    Task<SuccessResponse<EmployeePayrollInfoDto>> GetByIdAsync(long id);
    Task<SuccessResponse<EmployeePayrollInfoDto>> GetByEmployeeIdAsync(long employeeId);
    Task<SuccessResponse<long>> CreateAsync(CreateEmployeePayrollInfoDto dto);
    Task<SuccessResponse> UpdateAsync(long id, UpdateEmployeePayrollInfoDto dto);
    Task<SuccessResponse> DeleteAsync(long id);
}
