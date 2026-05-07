using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;

namespace EPMS.Domain.Interface.Irepo.Performance;

public interface IPositionPIPTemplateRepository : IGenericRepository<PositionPIPTemplate>
{
    Task<IEnumerable<PositionPIPTemplate>> GetByPositionIdAsync(long positionId);
    Task<IEnumerable<PositionPIPTemplate>> GetActiveByPositionIdAsync(long positionId);
}