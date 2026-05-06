using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;

namespace EPMS.Domain.Interface.IService.Info;

public interface IEmployeeProfileService
{
    Task<SuccessResponse<IEnumerable<EmployeeProfileDto>>> GetAllAsync();
    Task<SuccessResponse<EmployeeProfileDto>> GetByIdAsync(long id);
    Task<SuccessResponse<EmployeeProfileDetailDto>> GetFullProfileAsync(long id);
    Task<SuccessResponse<long>> CreateAsync(CreateEmployeeProfileDto dto);
    Task<SuccessResponse> UpdateAsync(long id, UpdateEmployeeProfileDto dto);
    Task<SuccessResponse> DeleteAsync(long id);
    Task<SuccessResponse<EmployeeProfileDto>> GetByStaffNoAsync(string staffNo);
    Task<SuccessResponse<EmployeeProfileDto>> GetByUserIdAsync(long userId);
}
