using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;

namespace EPMS.Domain.Interface.Irepo.Hr;

public interface IDepartmentRepository : IGenericRepository<Department>
{
    Task<Department?> GetDepartmentWithTeamsAsync(long id);
    Task<bool> ExistsByCodeAsync(string code);
    Task<bool> ExistsByNameAsync(string name);
    Task<bool> ExistsByIdAsync(long id);
    Task<IEnumerable<(long Id, string Name, bool IsActive)>> GetLookupAsync();
}
