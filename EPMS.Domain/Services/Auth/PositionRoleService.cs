using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Interface.Irepo.Auth;
using EPMS.Domain.Interface.IService.Auth;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static EPMS.Shared.Constants.ServiceResponseMessages;

namespace EPMS.Domain.Services.Auth;

public class PositionRoleService : IPositionRoleService
{
    private readonly IUnitOfWork _unitOfWork;

    public PositionRoleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<SuccessResponse<IEnumerable<PositionRoleDto>>> GetRolesForPositionAsync(long positionId)
    {
        var position = await _unitOfWork.HR.Positions.GetByIdAsync(positionId);
        if (position == null)
            return SuccessResponse<IEnumerable<PositionRoleDto>>.Fail(PositionMsg.NotFound(positionId), ErrorType.NotFound);

        if (position.IsDeleted)
            return SuccessResponse<IEnumerable<PositionRoleDto>>.Fail(PositionMsg.NotFound(positionId), ErrorType.NotFound);

        var positionRoles = await _unitOfWork.Auth.PositionRoles.GetByPositionIdAsync(positionId);

        var result = positionRoles.Select(pr => new PositionRoleDto
        {
            Id = pr.Id,
            PositionId = pr.PositionId,
            PositionName = position.Title,
            RoleId = pr.RoleId,
            RoleName = pr.Role?.Name ?? string.Empty,
            IsActive = pr.IsActive
        }).ToList();

        return SuccessResponse<IEnumerable<PositionRoleDto>>.Ok(result, PositionPermissionMsg.RetrievedByPosition);
    }

    public async Task<SuccessResponse<long>> AssignRoleToPositionAsync(long positionId, long roleId)
    {
        var position = await _unitOfWork.HR.Positions.GetByIdAsync(positionId);
        if (position == null || position.IsDeleted)
            return SuccessResponse<long>.Fail(PositionMsg.NotFound(positionId), ErrorType.NotFound);

        var role = await _unitOfWork.Auth.Roles.GetByIdAsync(roleId);
        if (role == null)
            return SuccessResponse<long>.Fail(PermissionMsg.NotFoundById(roleId), ErrorType.NotFound);

        var exists = await _unitOfWork.Auth.PositionRoles.ExistsAsync(positionId, roleId);
        if (exists)
            return SuccessResponse<long>.Fail(PositionPermissionMsg.DuplicateEntry, ErrorType.Conflict);

        var positionRole = new PositionRole(positionId, roleId);
        _unitOfWork.Auth.PositionRoles.Add(positionRole);
        await _unitOfWork.CompleteAsync();

        return SuccessResponse<long>.Ok(positionRole.Id, PositionPermissionMsg.Created);
    }

    public async Task<SuccessResponse> RemoveRoleFromPositionAsync(long positionId, long roleId)
    {
        var positionRole = await _unitOfWork.Auth.PositionRoles.GetByPositionAndRoleAsync(positionId, roleId);
        if (positionRole == null)
            return SuccessResponse.Fail(PositionPermissionMsg.NotFound(positionRole?.Id ?? 0), ErrorType.NotFound);

        _unitOfWork.Auth.PositionRoles.Delete(positionRole);
        await _unitOfWork.CompleteAsync();

        return SuccessResponse.Ok(PositionPermissionMsg.Deleted);
    }

    public async Task<SuccessResponse<IEnumerable<PositionRoleDto>>> GetPositionsForRoleAsync(long roleId)
    {
        var role = await _unitOfWork.Auth.Roles.GetByIdAsync(roleId);
        if (role == null)
            return SuccessResponse<IEnumerable<PositionRoleDto>>.Fail(PermissionMsg.NotFoundById(roleId), ErrorType.NotFound);

        var positionRoles = await _unitOfWork.Auth.PositionRoles.GetByRoleIdAsync(roleId);

        var result = positionRoles.Select(pr => new PositionRoleDto
        {
            Id = pr.Id,
            PositionId = pr.PositionId,
            PositionName = pr.Position?.Title ?? string.Empty,
            RoleId = pr.RoleId,
            RoleName = role.Name,
            IsActive = pr.IsActive
        }).ToList();

        return SuccessResponse<IEnumerable<PositionRoleDto>>.Ok(result, "Positions with this role retrieved successfully");
    }
}