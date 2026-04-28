using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities;
using EPMS.Domain.Entities.Hr;
using EPMS.Shared.PositionDTOs;

public class PositionService
{
    private readonly IGenericRepository<Position> _repo;
    private readonly IMapper _mapper;

    public PositionService(IGenericRepository<Position> repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PositionResponseDto>> GetAllAsync()
    {
        var data = await _repo.GetAllAsync();   

        return _mapper.Map<IEnumerable<PositionResponseDto>>(data);
    }

    public async Task<PositionResponseDto?> GetByIdAsync(int id)
    {
        var data = await _repo.GetByIdAsync(id);  

        return _mapper.Map<PositionResponseDto>(data);
    }

    public async Task<int> CreateAsync(CreatePositionDto dto)
    {
        var entity = _mapper.Map<Position>(dto);

        await _repo.AddAsync(entity);  

        return (int)entity.Id;
    }

    public async Task<bool> UpdateAsync(UpdatePositionDto dto)
    {
        var entity = await _repo.GetByIdAsync(dto.PositionId);

        if (entity == null) return false;

        _mapper.Map(dto, entity);

        _repo.Update(entity);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);

        if (entity == null) return false;

        _repo.Delete(entity);

        return true;
    }
}