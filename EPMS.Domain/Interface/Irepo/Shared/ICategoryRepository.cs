using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Shared;
using EPMS.Shared.DTOs.Common;

namespace EPMS.Domain.Interface.Irepo.Shared;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<IEnumerable<(long Id, string Code, bool IsActive)>> GetLookupAsync();
    Task<bool> ExistsByCodeAsync(string code, string module, long? excludeId = null);
    Task<bool> ExistsByNameAsync(string name, string module, long? excludeId = null);
    Task<bool> HasSubCategoriesAsync(long categoryId);
}
