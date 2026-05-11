using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.DepartmentDTOs;
using EPMS.Shared.DTOs.TeamDTOs;

namespace EPMS.Domain.Interfaces;

public interface IDepartmentService
{
    Task<SuccessResponse<IEnumerable<LookUpDto>>> GetLookupAsync();
    Task<SuccessResponse<IEnumerable<DepartmentDto>>> GetDepartmentWithTeamsAsync(long teamId);
    Task<SuccessResponse<IEnumerable<DepartmentDto>>> GetAllAsync();
    Task<SuccessResponse<DepartmentDto>> GetByIdAsync(long id);
    Task<SuccessResponse<long>> CreateAsync(CreateDepartmentDto dto);
    Task<SuccessResponse> UpdateAsync(long id, UpdateDepartmentDto dto);
    Task<SuccessResponse> DeleteAsync(long id);
}