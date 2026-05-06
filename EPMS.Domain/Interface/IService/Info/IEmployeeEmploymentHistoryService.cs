using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;

namespace EPMS.Domain.Interface.IService.Info;

public interface IEmployeeEmploymentHistoryService
{
    Task<SuccessResponse<IEnumerable<EmployeeEmploymentHistoryDto>>> GetAllAsync();
    Task<SuccessResponse<EmployeeEmploymentHistoryDto>> GetByIdAsync(long id);
    Task<SuccessResponse<IEnumerable<EmployeeEmploymentHistoryDto>>> GetByEmployeeIdAsync(long employeeId);
    Task<SuccessResponse<long>> CreateAsync(CreateEmployeeEmploymentHistoryDto dto);
    // No Update or Delete - history is append-only
}
