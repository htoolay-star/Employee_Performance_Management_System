using EPMS.Domain.Entities.Hr;

namespace EPMS.Domain.Interface.Irepo;

public interface IDepartmentRepository
{
    Task<IEnumerable<Department>> GetAllAsync();
    Task AddAsync(Department department);
    Task SaveChangesAsync();

}