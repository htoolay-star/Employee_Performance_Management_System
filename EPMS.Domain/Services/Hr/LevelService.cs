using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Interfaces;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.LevelDTOs;
using EPMS.Shared.Enums;
using static EPMS.Shared.Constants.ServiceResponseMessages;

namespace EPMS.Domain.Services.Hr;

public class LevelService : ILevelService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    public LevelService(IMapper mapper, IUnitOfWork uow)
    {
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<SuccessResponse<IEnumerable<LevelDto>>> GetAllAsync()
    {
        var levels = await _uow.HR.Levels.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<LevelDto>>(levels);
        return SuccessResponse<IEnumerable<LevelDto>>.Ok(dtos, ServiceResponseMessages.LevelMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<LevelDto>> GetByIdAsync(long id)
    {
        var level = await _uow.HR.Levels.GetByIdAsync(id);

        if (level is null)
            return SuccessResponse<LevelDto>.Fail(ServiceResponseMessages.LevelMsg.NotFound(id), ErrorType.NotFound);

        var dto = _mapper.Map<LevelDto>(level);
        return SuccessResponse<LevelDto>.Ok(dto, ServiceResponseMessages.LevelMsg.Retrieved);
    }

    public async Task<SuccessResponse<long>> CreateAsync(CreateLevelDto dto)
    {
        if (await _uow.HR.Levels.ExistsByCodeAsync(dto.Code))
            return SuccessResponse<long>.Fail(string.Format(ServiceResponseMessages.LevelMsg.DuplicateCode, dto.Code.Trim().ToUpperInvariant()), ErrorType.Conflict);

        var entity = new Level(dto.Code, dto.Name, dto.Description);
        _uow.HR.Levels.Add(entity);
        await _uow.CompleteAsync();
        return SuccessResponse<long>.Ok(entity.Id, ServiceResponseMessages.LevelMsg.Created);
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateLevelDto dto)
    {
        var level = await _uow.HR.Levels.GetByIdAsync(id);

        if (level is null)
            return SuccessResponse.Fail(ServiceResponseMessages.LevelMsg.NotFound(id), ErrorType.NotFound);

        level.Update(dto.Name, dto.Description);

        if (dto.IsActive) level.Reactivate();
        else level.Deactivate();

        await _uow.CompleteAsync();
        return SuccessResponse.Ok(ServiceResponseMessages.LevelMsg.Updated);
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var level = await _uow.HR.Levels.GetByIdAsync(id);

        if (level is null)
            return SuccessResponse.Fail(ServiceResponseMessages.LevelMsg.NotFound(id), ErrorType.NotFound);

        if (await _uow.HR.Levels.HasPositionsAsync(id))
            return SuccessResponse.Fail(ServiceResponseMessages.LevelMsg.NotFound(id), ErrorType.Conflict);

        _uow.HR.Levels.Delete(level);
        await _uow.CompleteAsync();
        return SuccessResponse.Ok(ServiceResponseMessages.LevelMsg.Deleted);
    }
}
