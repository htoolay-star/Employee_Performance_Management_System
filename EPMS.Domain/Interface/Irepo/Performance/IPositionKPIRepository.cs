using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Interface.Irepo.Performance
{
    public interface IPositionKPIRepository : IGenericRepository<PositionKPI>
    {
        Task<IEnumerable<PositionKPI>> GetByPositionIdAsync(long positionId);
        Task<PositionKPI?> GetByPositionAndKPIAsync(long positionId, long kpiId);
        Task<decimal> GetTotalWeightageByPositionAsync(long positionId, long? excludeKPIId = null);
    }
}
