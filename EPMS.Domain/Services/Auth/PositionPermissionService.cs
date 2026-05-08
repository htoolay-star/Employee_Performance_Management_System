using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Auth;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.AuthDTOs.PositionPermissionDTOs;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.Enums;
using static EPMS.Shared.Constants.ServiceResponseMessages;

namespace EPMS.Domain.Services.Auth;

public class PositionPermissionService : IPositionPermissionService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public PositionPermissionService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<SuccessResponse> CreateAsync(CreatePositionPermissionDto dto)
    {
        var exists = await _uow.Auth.PositionPermissions.ExistsAsync(dto.PositionId, dto.PermissionId);

        if (exists)
            return SuccessResponse.Fail(PositionPermissionMsg.DuplicateEntry, ErrorType.Conflict);

        var positionPermission = new PositionPermission(dto.PositionId, dto.PermissionId);

        _uow.Auth.PositionPermissions.Add(positionPermission);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok(PositionPermissionMsg.Created);
    }

    public async Task<SuccessResponse> DeleteAsync(long positionId, long permissionId)
    {
        var positionPermission = await _uow.Auth.PositionPermissions
            .GetByPositionAndPermissionAsync(positionId, permissionId);

        if (positionPermission == null)
            return SuccessResponse.Fail(
                PositionPermissionMsg.NotFoundByPositionAndPermission(positionId, permissionId),
                ErrorType.NotFound);

        _uow.Auth.PositionPermissions.Delete(positionPermission);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok(PositionPermissionMsg.Deleted);
    }

    public async Task<SuccessResponse> GetByIdAsync(long id)
    {
        var positionPermission = await _uow.Auth.PositionPermissions.GetByIdAsync(id);

        if (positionPermission == null)
            return SuccessResponse.Fail(PositionPermissionMsg.NotFound(id), ErrorType.NotFound);

        var dto = _mapper.Map<PositionPermissionDto>(positionPermission);
        return SuccessResponse<PositionPermissionDto>.Ok(dto, PositionPermissionMsg.Retrieved);
    }

    public async Task<SuccessResponse> GetAllAsync()
    {
        var positionPermissions = await _uow.Auth.PositionPermissions.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<PositionPermissionDto>>(positionPermissions);
        return SuccessResponse<IEnumerable<PositionPermissionDto>>.Ok(dtos, PositionPermissionMsg.RetrievedAll);
    }

    public async Task<SuccessResponse> GetByPositionIdAsync(long positionId)
    {
        var positionPermissions = await _uow.Auth.PositionPermissions
            .GetByPositionIdAsync(positionId);

        var dtos = _mapper.Map<IEnumerable<PositionPermissionDto>>(positionPermissions);
        return SuccessResponse<IEnumerable<PositionPermissionDto>>.Ok(dtos, PositionPermissionMsg.RetrievedByPosition);
    }

    public async Task<SuccessResponse> GetByPermissionIdAsync(long permissionId)
    {
        var positionPermissions = await _uow.Auth.PositionPermissions
            .GetByPermissionIdAsync(permissionId);

        var dtos = _mapper.Map<IEnumerable<PositionPermissionDto>>(positionPermissions);
        return SuccessResponse<IEnumerable<PositionPermissionDto>>.Ok(dtos, PositionPermissionMsg.RetrievedByPermission);
    }

    public async Task<SuccessResponse> GetPermissionsForPositionAsync(long positionId)
    {
        var permissions = await _uow.Auth.PositionPermissions
            .GetPermissionsForPositionAsync(positionId);

        return SuccessResponse<IEnumerable<Permission>>.Ok(permissions, PositionPermissionMsg.RetrievedByPosition);
    }
}
