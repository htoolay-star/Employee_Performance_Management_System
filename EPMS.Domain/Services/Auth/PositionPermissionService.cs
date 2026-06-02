using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Interface.IService.Auth;
using EPMS.Shared.DTOs.AuthDTOs.PositionPermissionDTOs;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.Enums;
using Mapster;
using static EPMS.Shared.Constants.ServiceResponseMessages;
namespace EPMS.Domain.Services.Auth;

public class PositionPermissionService : IPositionPermissionService
{
    private readonly IUnitOfWork _uow;

    public PositionPermissionService(IUnitOfWork uow)
    {
        _uow = uow;
    }


    public async Task<SuccessResponse> UpdatePositionPermissionsAsync(long positionId, List<long> selectedPermissionIds)
    {
        var existingPermissions = await _uow.Auth.PositionPermissions.GetByPositionIdAsync(positionId);

        foreach (var existing in existingPermissions)
        {
            _uow.Auth.PositionPermissions.Delete(existing);
        }

        if (selectedPermissionIds != null && selectedPermissionIds.Any())
        {
            foreach (var permissionId in selectedPermissionIds.Distinct())
            {
                var newPermission = new PositionPermission(positionId, permissionId);
                _uow.Auth.PositionPermissions.Add(newPermission);
            }
        }

        await _uow.CompleteAsync();

        return SuccessResponse.Ok("Position permissions updated successfully.");
    }

    public async Task<SuccessResponse> GetByIdAsync(long id)
    {
        var positionPermission = await _uow.Auth.PositionPermissions.GetByIdAsync(id);

        if (positionPermission == null)
            return SuccessResponse.Fail(PositionPermissionMsg.NotFound(id), ErrorType.NotFound);

        var dto = positionPermission.Adapt<PositionPermissionDto>();
        return SuccessResponse<PositionPermissionDto>.Ok(dto, PositionPermissionMsg.Retrieved);
    }

    public async Task<SuccessResponse> GetAllAsync()
    {
        var positionPermissions = await _uow.Auth.PositionPermissions.GetAllAsync();
        var dtos = positionPermissions.Adapt<IEnumerable<PositionPermissionDto>>();
        return SuccessResponse<IEnumerable<PositionPermissionDto>>.Ok(dtos, PositionPermissionMsg.RetrievedAll);
    }

    public async Task<SuccessResponse> GetByPositionIdAsync(long positionId)
    {
        var positionPermissions = await _uow.Auth.PositionPermissions
            .GetByPositionIdAsync(positionId);

        var dtos = positionPermissions.Adapt<IEnumerable<PositionPermissionDto>>();
        return SuccessResponse<IEnumerable<PositionPermissionDto>>.Ok(dtos, PositionPermissionMsg.RetrievedByPosition);
    }

    public async Task<SuccessResponse> GetByPermissionIdAsync(long permissionId)
    {
        var positionPermissions = await _uow.Auth.PositionPermissions
            .GetByPermissionIdAsync(permissionId);

        var dtos = positionPermissions.Adapt<IEnumerable<PositionPermissionDto>>();
        return SuccessResponse<IEnumerable<PositionPermissionDto>>.Ok(dtos, PositionPermissionMsg.RetrievedByPermission);
    }

    public async Task<SuccessResponse> GetPermissionsForPositionAsync(long positionId)
    {
        var permissions = await _uow.Auth.PositionPermissions
            .GetPermissionsForPositionAsync(positionId);

        return SuccessResponse<IEnumerable<Permission>>.Ok(permissions, PositionPermissionMsg.RetrievedByPosition);
    }
}
