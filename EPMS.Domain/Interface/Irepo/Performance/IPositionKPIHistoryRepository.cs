using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Interface.Irepo.Performance
{
    public interface IPositionKPIHistoryRepository : IGenericRepository<PositionKPIHistory>
    {
        Task<IEnumerable<PositionKPIHistory>> GetHistoryByPositionAsync(long positionId);
        Task<PositionKPIHistory?> GetLatestHistoryAsync(long positionId, long kpiId);
    }
}
