using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Interface.Irepo.Performance
{
    public interface IKPIMasterRepository : IGenericRepository<KPIMaster>
    {
        Task<bool> ExistsByCodeAsync(string code, long? excludeId = null);
        Task<IEnumerable<KPIMaster>> GetActiveKPIsAsync();
    }
}
