using EPMS.Domain.Entities.PerformanceImprovementPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Repositories.Interfaces
{
    public interface IPIPRepository
    {
        Task AddAsync(PerformanceImprovementPlan pip);
        Task<PerformanceImprovementPlan?> GetByIdAsync(Guid id);
        Task<List<PerformanceImprovementPlan>> GetAllAsync();
        Task SaveChangesAsync();
    }
}
