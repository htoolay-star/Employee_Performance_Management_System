using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;
using EPMS.Shared.DTOs.Common;

namespace EPMS.Domain.Interface.Irepo.Hr;

public interface ILevelRepository : IGenericRepository<Level>
{
    Task<bool> ExistsByCodeAsync(string code, long? excludeLevelId = null, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, long? excludeLevelId = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<LookUpDto>> GetLookupDtoAsync();
}
