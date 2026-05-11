using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;
using EPMS.Shared.DTOs.PositionDTOs;
using EPMS.Shared.Features.Positions;

namespace EPMS.Domain.Interface.Irepo.Hr;

public interface IPositionRepository : IGenericRepository<Position>
{
    Task<IEnumerable<Position>> GetAllWithLevelAsync(CancellationToken cancellationToken = default);
    Task<Position?> GetByIdWithLevelAsync(long id, bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, long? excludePositionId = null, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, long? excludePositionId = null, CancellationToken cancellationToken = default);
    Task<bool> ExistsByIdAsync(long id);
    Task<(IEnumerable<PositionGridItemDto> Items, int TotalCount)> GetPagedAsync(PositionQueryParameters parameters, string entitySortColumn, CancellationToken cancellationToken = default);
}
