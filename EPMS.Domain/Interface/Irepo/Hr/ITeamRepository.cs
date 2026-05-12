using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.TeamDTOs;
using EPMS.Shared.Features.Teams;

namespace EPMS.Domain.Interface.Irepo.Hr;

public interface ITeamRepository : IGenericRepository<Team>
{
    Task<IEnumerable<Team>> GetTeamsByDepartmentAsync(long departmentId);
    Task<bool> ExistsByCodeAsync(string code, long? excludeId = null);
    Task<bool> ExistsByNameInDepartmentAsync(string name, long departmentId, long? excludeId = null);
    Task<bool> ExistsByIdAsync(long id);
    Task<IEnumerable<LookUpDto>> GetLookupDtoAsync();
    Task<(IEnumerable<TeamGridItemDto> Items, int TotalCount)> GetPagedAsync(TeamQueryParameters parameters, string entitySortColumn, CancellationToken cancellationToken = default);
}
