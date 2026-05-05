using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;

namespace EPMS.Domain.Interface.Irepo.Hr;

public interface ILevelRepository : IGenericRepository<Level>
{
    Task<bool> ExistsByCodeAsync(string code, long? excludeLevelId = null, CancellationToken cancellationToken = default);
    Task<bool> HasPositionsAsync(long levelId, CancellationToken cancellationToken = default);
}
