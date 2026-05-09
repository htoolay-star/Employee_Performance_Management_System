using EPMS.Shared.DTOs.AuthDTOs.PermissionDTOS;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PositionDTOs;
using EPMS.Shared.Features.Positions;

namespace EPMS.Domain.Interfaces;

public interface IPositionService
{
    Task<SuccessResponse<IEnumerable<PositionDto>>> GetAllAsync();
    Task<SuccessResponse<PositionDto>> GetByIdAsync(long id);
    Task<SuccessResponse<long>> CreateAsync(CreatePositionDto dto);
    Task<SuccessResponse> UpdateAsync(long id, UpdatePositionDto dto);
    Task<SuccessResponse> DeleteAsync(long id);
    Task<SuccessResponse<PaginatedResponse<PositionGridItemDto>>> GetPagedAsync(PositionQueryParameters parameters);

    // Permission management methods
    Task<SuccessResponse<IEnumerable<PermissionDto>>> GetPermissionsForPositionAsync(long positionId);
    Task<SuccessResponse> AssignPermissionToPositionAsync(long positionId, long permissionId);
    Task<SuccessResponse> RemovePermissionFromPositionAsync(long positionId, long permissionId);
}
