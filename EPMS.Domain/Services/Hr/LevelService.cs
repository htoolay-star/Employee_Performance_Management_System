using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Interfaces;
using EPMS.Shared.DTOs.LevelDTOs;

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

    public async Task<IEnumerable<LevelDto>> GetAllAsync()
    {
        var levels = await _uow.HR.Levels.GetAllAsync();
        return _mapper.Map<IEnumerable<LevelDto>>(levels);
    }

    public async Task<LevelDto?> GetByIdAsync(int id)
    {
        var level = await _uow.HR.Levels.GetByIdAsync(id);

        if (level is null)
            throw new KeyNotFoundException($"Level with ID '{id}' was not found.");

        return _mapper.Map<LevelDto>(level);
    }

    public async Task<int> CreateAsync(CreateLevelDto dto)
    {
        if (await _uow.HR.Levels.ExistsByCodeAsync(dto.Code))
            throw new InvalidOperationException($"Level with code '{dto.Code.Trim().ToUpperInvariant()}' already exists.");

        var entity = new Level(dto.Code, dto.Name, dto.Description);
        _uow.HR.Levels.Add(entity);
        await _uow.CompleteAsync();
        return checked((int)entity.Id);
    }

    public async Task UpdateAsync(int id, UpdateLevelDto dto)
    {
        var level = await _uow.HR.Levels.GetByIdAsync(id);

        if (level is null)
            throw new KeyNotFoundException($"Level with ID '{id}' was not found.");

        level.Update(dto.Name, dto.Description);

        if (dto.IsActive) level.Reactivate();
        else level.Deactivate();

        await _uow.CompleteAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var level = await _uow.HR.Levels.GetByIdAsync(id);

        if (level is null)
            throw new KeyNotFoundException($"Level with ID '{id}' was not found.");

        if (await _uow.HR.Levels.HasPositionsAsync(id))
            throw new InvalidOperationException($"Cannot delete level '{id}' because one or more positions are assigned to it.");

        _uow.HR.Levels.Delete(level);
        await _uow.CompleteAsync();
    }
}
