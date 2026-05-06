using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;

namespace EPMS.Domain.Interface.IService.Info;

public interface IEmployeeEmploymentService
{
    Task<SuccessResponse<IEnumerable<EmployeeEmploymentDto>>> GetAllAsync();
    Task<SuccessResponse<EmployeeEmploymentDto>> GetByIdAsync(long id);
    Task<SuccessResponse<EmployeeEmploymentDto>> GetByEmployeeIdAsync(long employeeId);
    Task<SuccessResponse<long>> CreateAsync(CreateEmployeeEmploymentDto dto);
    Task<SuccessResponse> UpdateAsync(long id, UpdateEmployeeEmploymentDto dto);
    Task<SuccessResponse> DeleteAsync(long id);
}
