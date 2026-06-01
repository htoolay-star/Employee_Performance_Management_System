using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;

namespace EPMS.Domain.Interface.IService.Info;

public interface IEmployeeProfileService
{
    Task<SuccessResponse<IEnumerable<EmployeeProfileDto>>> GetAllAsync();
    Task<SuccessResponse<EmployeeProfileDto>> GetByIdAsync(long id);
    Task<SuccessResponse<EmployeeProfileDto>> GetByPublicIdAsync(Guid publicId);
    Task<SuccessResponse<long>> CreateAsync(CreateEmployeeProfileDto dto, string? preHashedPassword = null);
    Task<SuccessResponse<long>> CreateFullAsync(CreateFullEmployeeDto dto, string? preHashedPassword = null);
    Task<SuccessResponse> UpdateAsync(long id, UpdateEmployeeProfileDto dto);
    Task<SuccessResponse> DeleteAsync(long id);
    Task<SuccessResponse<EmployeeProfileDto>> GetByStaffNoAsync(string staffNo);
    Task<SuccessResponse<EmployeeProfileDto>> GetByUserIdAsync(long userId);
    Task<SuccessResponse<EmployeeProfileDto>> GetMyProfileAsync();
    Task<SuccessResponse<IEnumerable<EmployeeLookupDto>>> GetLookupAsync();
    Task<SuccessResponse<IEnumerable<EmployeeLookupDto>>> GetDirectReportsLookupAsync();
    Task<SuccessResponse<PaginatedResponse<EmployeeProfileGridItemDto>>> GetPagedAsync(EPMS.Shared.Features.EmployeeProfiles.EmployeeProfileQueryParameters parameters);

    Task<SuccessResponse<IEnumerable<EmployeeFullImportRow>>> GetFullExportAsync();
    Task<SuccessResponse<ImportResult>> ImportFullEmployeesAsync(List<EmployeeFullImportRow> rows);
    Task<SuccessResponse<ImportPreviewResult>> ImportPreviewAsync(List<EmployeeFullImportRow> rows);
}
