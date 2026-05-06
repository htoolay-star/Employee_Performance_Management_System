using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;

namespace EPMS.Domain.Interface.IService.Info;

public interface IEmployeeContactService
{
    Task<SuccessResponse<IEnumerable<EmployeeContactDto>>> GetAllAsync();
    Task<SuccessResponse<EmployeeContactDto>> GetByIdAsync(long id);
    Task<SuccessResponse<EmployeeContactDto>> GetByEmployeeIdAsync(long employeeId);
    Task<SuccessResponse<long>> CreateAsync(CreateEmployeeContactDto dto);
    Task<SuccessResponse> UpdateAsync(long id, UpdateEmployeeContactDto dto);
    Task<SuccessResponse> DeleteAsync(long id);
}
