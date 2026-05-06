using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Interfaces;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.AuthDTOs.PermissionDTOS;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PositionDTOs;
using EPMS.Shared.Enums;
using static EPMS.Shared.Constants.ServiceResponseMessages;

namespace EPMS.Domain.Services.Hr;

public class PositionService : IPositionService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    public PositionService(IMapper mapper, IUnitOfWork uow)
    {
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<SuccessResponse<IEnumerable<PositionDto>>> GetAllAsync()
    {
        var positions = await _uow.HR.Positions.GetAllWithLevelAsync();
        var dtos = _mapper.Map<IEnumerable<PositionDto>>(positions);
        return SuccessResponse<IEnumerable<PositionDto>>.Ok(dtos, ServiceResponseMessages.PositionMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<PositionDto>> GetByIdAsync(long id)
    {
        var position = await _uow.HR.Positions.GetByIdWithLevelAsync(id);

        if (position is null)
            return SuccessResponse<PositionDto>.Fail(ServiceResponseMessages.PositionMsg.NotFound(id), ErrorType.NotFound);

        var dto = _mapper.Map<PositionDto>(position);
        return SuccessResponse<PositionDto>.Ok(dto, ServiceResponseMessages.PositionMsg.Retrieved);
    }

    public async Task<SuccessResponse<long>> CreateAsync(CreatePositionDto dto)
    {
        if (!await _uow.HR.Positions.LevelExistsAsync(dto.LevelId))
            return SuccessResponse<long>.Fail(ServiceResponseMessages.PositionMsg.LevelNotFound(dto.LevelId), ErrorType.NotFound);

        if (await _uow.HR.Positions.ExistsByTitleAsync(dto.Title))
            return SuccessResponse<long>.Fail(string.Format(ServiceResponseMessages.PositionMsg.DuplicateTitle, dto.Title.Trim()), ErrorType.Conflict);

        var entity = new Position(dto.Title, dto.LevelId);
        _uow.HR.Positions.Add(entity);
        await _uow.CompleteAsync();
        return SuccessResponse<long>.Ok(entity.Id, ServiceResponseMessages.PositionMsg.Created);
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdatePositionDto dto)
    {
        var position = await _uow.HR.Positions.GetByIdAsync(id);

        if (position is null)
            return SuccessResponse.Fail(ServiceResponseMessages.PositionMsg.NotFound(id), ErrorType.NotFound);

        if (!await _uow.HR.Positions.LevelExistsAsync(dto.LevelId))
            return SuccessResponse.Fail(ServiceResponseMessages.PositionMsg.LevelNotFound(dto.LevelId), ErrorType.NotFound);

        if (position.Title != dto.Title.Trim() && await _uow.HR.Positions.ExistsByTitleAsync(dto.Title, id))
            return SuccessResponse.Fail(string.Format(ServiceResponseMessages.PositionMsg.DuplicateTitle, dto.Title.Trim()), ErrorType.Conflict);

        position.Update(dto.Title, dto.LevelId);

        if (dto.IsActive) position.Reactivate();
        else position.Deactivate();

        await _uow.CompleteAsync();
        return SuccessResponse.Ok(ServiceResponseMessages.PositionMsg.Updated);
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var position = await _uow.HR.Positions.GetByIdAsync(id);

        if (position is null)
            return SuccessResponse.Fail(ServiceResponseMessages.PositionMsg.NotFound(id), ErrorType.NotFound);

        _uow.HR.Positions.Delete(position);
        await _uow.CompleteAsync();
        return SuccessResponse.Ok(ServiceResponseMessages.PositionMsg.Deleted);
    }

    public async Task<SuccessResponse<IEnumerable<PermissionDto>>> GetPermissionsForPositionAsync(long positionId)
    {
        var position = await _uow.HR.Positions.GetByIdAsync(positionId);

        if (position is null)
            return SuccessResponse<IEnumerable<PermissionDto>>.Fail(ServiceResponseMessages.PositionMsg.NotFound(positionId), ErrorType.NotFound);

        var permissions = await _uow.Auth.PositionPermissions.GetPermissionsForPositionAsync(positionId);
        var dtos = _mapper.Map<IEnumerable<PermissionDto>>(permissions);
        return SuccessResponse<IEnumerable<PermissionDto>>.Ok(dtos, PermissionMsg.RetrievedAll);
    }

    public async Task<SuccessResponse> AssignPermissionToPositionAsync(long positionId, long permissionId)
    {
        var position = await _uow.HR.Positions.GetByIdAsync(positionId);

        if (position is null)
            return SuccessResponse.Fail(ServiceResponseMessages.PositionMsg.NotFound(positionId), ErrorType.NotFound);

        var permission = await _uow.Auth.Permissions.GetByIdAsync(permissionId);

        if (permission is null)
            return SuccessResponse.Fail(PermissionMsg.NotFoundById(permissionId), ErrorType.NotFound);

        if (await _uow.Auth.PositionPermissions.ExistsAsync(positionId, permissionId))
            return SuccessResponse.Fail(PermissionMsg.DuplicateName, ErrorType.Conflict);

        var positionPermission = new PositionPermission(positionId, permissionId);
        _uow.Auth.PositionPermissions.Add(positionPermission);
        await _uow.CompleteAsync();
        return SuccessResponse.Ok(ServiceResponseMessages.PositionMsg.PermissionAssigned);
    }

    public async Task<SuccessResponse> RemovePermissionFromPositionAsync(long positionId, long permissionId)
    {
        var positionPermission = await _uow.Auth.PositionPermissions.GetByPositionAndPermissionAsync(positionId, permissionId);

        if (positionPermission is null)
            return SuccessResponse.Fail(PermissionMsg.NotFound, ErrorType.NotFound);

        _uow.Auth.PositionPermissions.Delete(positionPermission);
        await _uow.CompleteAsync();
        return SuccessResponse.Ok(ServiceResponseMessages.PositionMsg.PermissionRemoved);
    }
}
