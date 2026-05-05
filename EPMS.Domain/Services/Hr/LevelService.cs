using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Interfaces;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.LevelDTOs;
using EPMS.Shared.Enums;

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
        return SuccessResponse<IEnumerable<LevelDto>>.Ok(dtos);
    }

    public async Task<SuccessResponse<LevelDto>> GetByIdAsync(int id)
    {
        var level = await _uow.HR.Levels.GetByIdAsync(id);

        if (level is null)
            return SuccessResponse<LevelDto>.Fail($"Level with ID '{id}' was not found.", ErrorType.NotFound);

        var dto = _mapper.Map<LevelDto>(level);
        return SuccessResponse<LevelDto>.Ok(dto);
    }

    public async Task<SuccessResponse<int>> CreateAsync(CreateLevelDto dto)
    {
        if (await _uow.HR.Levels.ExistsByCodeAsync(dto.Code))
            return SuccessResponse<int>.Fail($"Level with code '{dto.Code.Trim().ToUpperInvariant()}' already exists.", ErrorType.Conflict);

        var entity = new Level(dto.Code, dto.Name, dto.Description);
        _uow.HR.Levels.Add(entity);
        await _uow.CompleteAsync();
        return SuccessResponse<int>.Ok(checked((int)entity.Id), "Level created successfully.");
    }

    public async Task<SuccessResponse> UpdateAsync(int id, UpdateLevelDto dto)
    {
        var level = await _uow.HR.Levels.GetByIdAsync(id);

        if (level is null)
            return SuccessResponse.Fail($"Level with ID '{id}' was not found.", ErrorType.NotFound);

        level.Update(dto.Name, dto.Description);

        if (dto.IsActive) level.Reactivate();
        else level.Deactivate();

        await _uow.CompleteAsync();
        return SuccessResponse.Ok("Level updated successfully.");
    }

    public async Task<SuccessResponse> DeleteAsync(int id)
    {
        var level = await _uow.HR.Levels.GetByIdAsync(id);

        if (level is null)
            return SuccessResponse.Fail($"Level with ID '{id}' was not found.", ErrorType.NotFound);

        if (await _uow.HR.Levels.HasPositionsAsync(id))
            return SuccessResponse.Fail($"Cannot delete level '{id}' because one or more positions are assigned to it.", ErrorType.Conflict);

        _uow.HR.Levels.Delete(level);
        await _uow.CompleteAsync();
        return SuccessResponse.Ok("Level deleted successfully.");
    }
}
