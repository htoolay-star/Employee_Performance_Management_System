using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Shared;

namespace EPMS.Domain.Interface.Irepo.Shared;

public interface ITagRepository : IGenericRepository<Tag>
{
    Task<IEnumerable<(long Id, string Code)>> GetLookupAsync();
    Task<bool> ExistsByNameAsync(string name, string? module = null);
    Task<bool> ExistsByNameAsync(string name, string? module, long excludeId);
}
