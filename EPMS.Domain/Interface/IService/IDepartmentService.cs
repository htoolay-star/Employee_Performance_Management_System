using EPMS.Shared.DTOs.HR;

namespace EPMS.Domain.Interfaces;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentDto>> GetAllAsync();
    Task<DepartmentDto?> GetByIdAsync(long id); // Fetch single record
    Task<long> CreateAsync(DepartmentDto dto);
    Task UpdateAsync(long id, DepartmentDto dto); // Update record
    Task DeleteAsync(long id); // Delete record
}