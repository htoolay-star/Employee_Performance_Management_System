using EPMS.Shared.DTOs.FormDTOs;
using EPMS.Shared.DTOs.Common;

namespace EPMS.Domain.Interface.IService;

public interface IAppraisalService
{
    Task<SuccessResponse> CreateAsync(CreateAppraisalDto dto);
    Task<SuccessResponse> UpdateAsync(long id, UpdateAppraisalDto dto);
    Task<SuccessResponse> DeleteAsync(long id);
    Task<SuccessResponse> GetByIdAsync(long id);
    Task<SuccessResponse> GetAllAsync();
    Task<SuccessResponse> GetByEmployeeIdAsync(long employeeId);
    Task<SuccessResponse> SubmitAsync(AppraisalSubmissionDto dto);
    Task<SuccessResponse> LockAsync(long id, long adminId, string reason);
    Task<SuccessResponse> UnlockAsync(long id, long adminId, string reason);
}