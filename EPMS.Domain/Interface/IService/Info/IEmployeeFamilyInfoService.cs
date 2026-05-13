using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;

namespace EPMS.Domain.Interface.IService.Info;

public interface IEmployeeFamilyInfoService
{
    Task<SuccessResponse<IEnumerable<EmployeeFamilyInfoDto>>> GetAllAsync();
    Task<SuccessResponse<EmployeeFamilyInfoDto>> GetByIdAsync(long id);
    Task<SuccessResponse<EmployeeFamilyInfoDto>> GetByEmployeeIdAsync(Guid employeePublicId);
    Task<SuccessResponse<long>> CreateAsync(CreateEmployeeFamilyInfoDto dto);
    Task<SuccessResponse> UpdateAsync(long id, UpdateEmployeeFamilyInfoDto dto);
    Task<SuccessResponse> DeleteAsync(long id);
}
