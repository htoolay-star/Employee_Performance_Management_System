using AutoMapper;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Interface.Irepo;
using EPMS.Domain.Interfaces;
using EPMS.Shared.DTOs.HR;

namespace EPMS.Domain.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _repo;
    private readonly IMapper _mapper;

    public DepartmentService(IDepartmentRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
    {
        var departments = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<DepartmentDto>>(departments);
    }

    public async Task<DepartmentDto?> GetByIdAsync(long id)
    {
        var department = await _repo.GetByIdAsync(id);
        return _mapper.Map<DepartmentDto>(department);
    }

    public async Task<long> CreateAsync(CreateDepartmentDto dto)
    {
        if (await _repo.ExistsByCodeAsync(dto.Code))
        {
            throw new InvalidOperationException($"Department with code '{dto.Code}' already exists.");
        }

        if (await _repo.ExistsByNameAsync(dto.Name))
        {
            throw new InvalidOperationException($"Department with name '{dto.Name}' already exists.");
        }

        var entity = new Department(dto.Code, dto.Name);
        await _repo.AddAsync(entity);
        await _repo.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateAsync(long id, DepartmentDto dto)
    {
        var department = await _repo.GetByIdAsync(id);
        if (department == null) return;

        if (department.Name != dto.Name && await _repo.ExistsByNameAsync(dto.Name))
        {
            throw new InvalidOperationException($"Another department with name '{dto.Name}' already exists.");
        }

        department.Rename(dto.Name);
        
        if (dto.IsActive) department.Reactivate();
        else department.Deactivate();

        _repo.Update(department);
        await _repo.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var department = await _repo.GetByIdAsync(id);
        if (department != null)
        {
            _repo.Delete(department);
            await _repo.SaveChangesAsync();
        }
    }
}
