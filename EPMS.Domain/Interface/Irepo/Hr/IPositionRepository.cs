using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;

namespace EPMS.Domain.Interface.Irepo.Hr;

public interface IPositionRepository : IGenericRepository<Position>
{
    Task<IEnumerable<Position>> GetAllWithLevelAsync(CancellationToken cancellationToken = default);
    Task<Position?> GetByIdWithLevelAsync(long id, bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<bool> ExistsByTitleAsync(string title, long? excludePositionId = null, CancellationToken cancellationToken = default);
    Task<bool> LevelExistsAsync(int levelId, CancellationToken cancellationToken = default);
}
