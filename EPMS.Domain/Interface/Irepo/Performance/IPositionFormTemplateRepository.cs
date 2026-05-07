using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;

namespace EPMS.Domain.Interface.Irepo.Performance;

public interface IPositionFormTemplateRepository : IGenericRepository<PositionFormTemplate>
{
    Task<IEnumerable<PositionFormTemplate>> GetByPositionIdAsync(long positionId);
    Task<bool> ExistsAsync(long positionId, long formTemplateId);
}