using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Interface.Irepo.Hr;
using EPMS.Domain.Interfaces;
using EPMS.Shared.DTOs.HR;

namespace EPMS.Domain.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    public DepartmentService(IMapper mapper, IUnitOfWork uow)
    {
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
    {
        var departments = await _uow.HR.Departments.GetAllAsync();
        return _mapper.Map<IEnumerable<DepartmentDto>>(departments);
    }

    public async Task<DepartmentDto?> GetByIdAsync(long id)
    {
        var department = await _uow.HR.Departments.GetByIdAsync(id);

        if (department is null)
            throw new KeyNotFoundException($"Department with ID '{id}' was not found.");

        return _mapper.Map<DepartmentDto>(department);
    }

    public async Task<long> CreateAsync(CreateDepartmentDto dto)
    {
        if (await _uow.HR.Departments.ExistsByCodeAsync(dto.Code))
        {
            throw new InvalidOperationException($"Department with code '{dto.Code}' already exists.");
        }

        if (await _uow.HR.Departments.ExistsByNameAsync(dto.Name))
        {
            throw new InvalidOperationException($"Department with name '{dto.Name}' already exists.");
        }

        var entity = new Department(dto.Code, dto.Name);
        _uow.HR.Departments.Add(entity);
        await _uow.CompleteAsync();
        return entity.Id;
    }

    public async Task UpdateAsync(long id, UpdateDepartmentDto dto)
    {
        var department = await _uow.HR.Departments.GetByIdAsync(id);

        if (department == null)
            throw new KeyNotFoundException($"Department with ID '{id}' was not found.");

        if (department.Name != dto.Name && await _uow.HR.Departments.ExistsByNameAsync(dto.Name))
        {
            throw new InvalidOperationException($"Another department with name '{dto.Name}' already exists.");
        }

        department.Rename(dto.Name);
        
        if (dto.IsActive) department.Reactivate();
        else department.Deactivate();

        await _uow.CompleteAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var department = await _uow.HR.Departments.GetByIdAsync(id);

        if (department == null)
            throw new KeyNotFoundException($"Department with ID '{id}' was not found.");

        if (department != null)
        {
            _uow.HR.Departments.Delete(department);
            await _uow.CompleteAsync();
        }
    }
}
