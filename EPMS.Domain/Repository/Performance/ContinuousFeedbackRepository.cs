using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Performance
{
    public class ContinuousFeedbackRepository : GenericRepository<ContinuousFeedback>, IContinuousFeedbackRepository
    {
        public ContinuousFeedbackRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<ContinuousFeedback>> GetByEmployeeIdAsync(long employeeId)
        {
            return await _dbSet
                .Where(x => x.EmployeeId == employeeId && !x.IsDeleted)
                .Include(x => x.GivenBy)
                .OrderByDescending(x => x.FeedbackDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ContinuousFeedback>> GetGivenByUserIdAsync(long userId)
        {
            return await _dbSet
                .Where(x => x.GivenById == userId && !x.IsDeleted)
                .Include(x => x.Employee)
                .OrderByDescending(x => x.FeedbackDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ContinuousFeedback>> GetAllWithIncludesAsync()
        {
            return await _dbSet
                .Where(x => !x.IsDeleted)
                .Include(x => x.Employee)
                .Include(x => x.GivenBy)
                .OrderByDescending(x => x.FeedbackDate)
                .ToListAsync();
        }
    }
}