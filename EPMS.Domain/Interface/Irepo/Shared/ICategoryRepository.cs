using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Shared;

namespace EPMS.Domain.Interface.Irepo.Shared;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<bool> ExistsByCodeAsync(string code, string module, int? excludeId = null);
    Task<bool> ExistsByNameAsync(string name, string module, int? excludeId = null);
    Task<bool> HasSubCategoriesAsync(int categoryId);
}
