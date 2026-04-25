using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;

namespace EPMS.Domain.Interface.Irepo.Hr;

public interface ITeamRepository : IGenericRepository<Team>
{
    Task<IEnumerable<Team>> GetTeamsByDepartmentAsync(long departmentId);
    Task<bool> ExistsByNameInDepartmentAsync(string name, long departmentId);
}
