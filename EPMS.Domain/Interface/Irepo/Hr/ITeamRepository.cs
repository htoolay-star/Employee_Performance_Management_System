using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;
using EPMS.Shared.DTOs.TeamDTOs;
using EPMS.Shared.Features.Teams;

namespace EPMS.Domain.Interface.Irepo.Hr;

public interface ITeamRepository : IGenericRepository<Team>
{
    Task<IEnumerable<Team>> GetTeamsByDepartmentAsync(long departmentId);
    Task<bool> ExistsByCodeAsync(string code, long? excludeId = null);
    Task<bool> ExistsByNameInDepartmentAsync(string name, long departmentId, long? excludeId = null);
    Task<bool> ExistsByIdAsync(long id);
    Task<IEnumerable<(long Id, string Code, bool IsActive)>> GetLookupAsync();
    Task<(IEnumerable<TeamGridItemDto> Items, int TotalCount)> GetPagedAsync(TeamQueryParameters parameters, string entitySortColumn, CancellationToken cancellationToken = default);
}
