using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;

namespace EPMS.Domain.Interface.Irepo.Performance
{
    public interface IFormTemplateRepository : IGenericRepository<FormTemplate>
    {
        Task<IEnumerable<FormTemplate>> GetActiveAsync();
        Task<FormTemplate?> GetByNameAsync(string name);
        Task<bool> NameExistsAsync(string name, long? excludeId = null);
    }
}