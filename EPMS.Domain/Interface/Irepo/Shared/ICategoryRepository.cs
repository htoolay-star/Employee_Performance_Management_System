using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Shared;
using EPMS.Shared.DTOs.Common;

namespace EPMS.Domain.Interface.Irepo.Shared;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<IEnumerable<LookUpDto>> GetLookupAsync();
    Task<bool> ExistsByCodeAsync(string code, long? excludeId = null);
    Task<bool> ExistsByNameAsync(string name, long? excludeId = null);
    Task<bool> HasSubCategoriesAsync(long categoryId);
}
