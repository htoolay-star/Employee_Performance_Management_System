using EPMS.Domain.Entities.PerformanceReview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Repositories.Interfaces
{
    public interface IPerformanceReviewRepository
    {
        Task AddAsync(PerformanceReview review);
        Task<List<PerformanceReview>> GetAllAsync();
        Task<PerformanceReview?> GetByIdAsync(Guid id);
    }
}
