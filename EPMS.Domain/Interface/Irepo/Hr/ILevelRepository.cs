using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;

namespace EPMS.Domain.Interface.Irepo.Hr;

public interface ILevelRepository : IGenericRepository<Level>
{
    Task<bool> ExistsByCodeAsync(string code, int? excludeLevelId = null, CancellationToken cancellationToken = default);
    Task<bool> HasPositionsAsync(int levelId, CancellationToken cancellationToken = default);
}
