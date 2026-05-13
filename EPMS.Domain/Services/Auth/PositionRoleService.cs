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

    public async Task<SuccessResponse<IEnumerable<long>>> GetAdminPositionIdsAsync()
    {
        long adminRoleId = (long)UserRole.Admin;

        var adminPositions = await _unitOfWork.Auth.PositionRoles.GetByRoleIdAsync(adminRoleId);
        var positionIds = adminPositions.Select(pr => pr.PositionId).ToList();

        return SuccessResponse<IEnumerable<long>>.Ok(positionIds, "Admin positions retrieved successfully.");
    }

    public async Task<SuccessResponse> ToggleAdminRoleAsync(long positionId, bool isGrantingAdmin)
    {
        var position = await _unitOfWork.HR.Positions.GetByIdAsync(positionId);
        if (position == null || position.IsDeleted)
            return SuccessResponse.Fail("Position not found.", ErrorType.NotFound);

        long adminRoleId = (long)UserRole.Admin;
        var existingRole = await _unitOfWork.Auth.PositionRoles.GetByPositionAndRoleAsync(positionId, adminRoleId);

        if (isGrantingAdmin && existingRole == null)
        {
            var positionRole = new PositionRole(positionId, adminRoleId);
            _unitOfWork.Auth.PositionRoles.Add(positionRole);
        }
        else if (!isGrantingAdmin && existingRole != null)
        {
            _unitOfWork.Auth.PositionRoles.Delete(existingRole);
        }

        await _unitOfWork.CompleteAsync();

        var message = isGrantingAdmin ? "Admin role granted successfully." : "Admin role revoked successfully.";
        return SuccessResponse.Ok(message);
    }
}