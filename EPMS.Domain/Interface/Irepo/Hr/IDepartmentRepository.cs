using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;
using EPMS.Shared.DTOs.Common;

namespace EPMS.Domain.Interface.Irepo.Hr;

public interface IDepartmentRepository : IGenericRepository<Department>
{
    Task<Department?> GetDepartmentWithTeamsAsync(long id);
    Task<bool> ExistsByCodeAsync(string code);
    Task<bool> ExistsByNameAsync(string name);
    Task<bool> ExistsByNameAsync(string name, long excludeId);
    Task<bool> ExistsByIdAsync(long id);
    Task<IEnumerable<LookUpDto>> GetLookupDtoAsync();
}
