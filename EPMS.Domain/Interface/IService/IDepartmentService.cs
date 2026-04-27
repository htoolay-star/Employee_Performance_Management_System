using EPMS.Shared.DTOs.HR;

namespace EPMS.Domain.Interfaces;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentDto>> GetAllAsync();
    Task<DepartmentDto?> GetByIdAsync(long id);
    Task<long> CreateAsync(CreateDepartmentDto dto);
    Task UpdateAsync(long id, UpdateDepartmentDto dto);
    Task DeleteAsync(long id);
}