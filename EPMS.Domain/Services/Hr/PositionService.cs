using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Interfaces;
using EPMS.Shared.DTOs.PositionDTOs;

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

    public async Task<IEnumerable<PositionDto>> GetAllAsync()
    {
        var positions = await _uow.HR.Positions.GetAllWithLevelAsync();
        return _mapper.Map<IEnumerable<PositionDto>>(positions);
    }

    public async Task<PositionDto?> GetByIdAsync(long id)
    {
        var position = await _uow.HR.Positions.GetByIdWithLevelAsync(id);

        if (position is null)
            throw new KeyNotFoundException($"Position with ID '{id}' was not found.");

        return _mapper.Map<PositionDto>(position);
    }

    public async Task<long> CreateAsync(CreatePositionDto dto)
    {
        if (!await _uow.HR.Positions.LevelExistsAsync(dto.LevelId))
            throw new InvalidOperationException($"Level with ID '{dto.LevelId}' was not found.");

        if (await _uow.HR.Positions.ExistsByTitleAsync(dto.Title))
            throw new InvalidOperationException($"A position with title '{dto.Title.Trim()}' already exists.");

        var entity = new Position(dto.Title, dto.LevelId);
        _uow.HR.Positions.Add(entity);
        await _uow.CompleteAsync();
        return entity.Id;
    }

    public async Task UpdateAsync(long id, UpdatePositionDto dto)
    {
        var position = await _uow.HR.Positions.GetByIdAsync(id);

        if (position is null)
            throw new KeyNotFoundException($"Position with ID '{id}' was not found.");

        if (!await _uow.HR.Positions.LevelExistsAsync(dto.LevelId))
            throw new InvalidOperationException($"Level with ID '{dto.LevelId}' was not found.");

        if (position.Title != dto.Title.Trim() && await _uow.HR.Positions.ExistsByTitleAsync(dto.Title, id))
            throw new InvalidOperationException($"Another position with title '{dto.Title.Trim()}' already exists.");

        position.Update(dto.Title, dto.LevelId);

        if (dto.IsActive) position.Reactivate();
        else position.Deactivate();

        await _uow.CompleteAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var position = await _uow.HR.Positions.GetByIdAsync(id);

        if (position is null)
            throw new KeyNotFoundException($"Position with ID '{id}' was not found.");

        _uow.HR.Positions.Delete(position);
        await _uow.CompleteAsync();
    }
}
