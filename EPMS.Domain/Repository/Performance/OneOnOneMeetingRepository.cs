using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;
using static EPMS.Shared.Constants.MeetingStatuses;

namespace EPMS.Domain.Repository.Performance
{
    public class OneOnOneMeetingRepository : GenericRepository<OneOnOneMeeting>, IOneOnOneMeetingRepository
    {
        public OneOnOneMeetingRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<OneOnOneMeeting>> GetByEmployeeIdAsync(long employeeId)
        {
            return await _dbSet
                .Where(x => x.EmployeeId == employeeId && !x.IsDeleted)
                .Include(x => x.Manager)
                .OrderByDescending(x => x.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<OneOnOneMeeting>> GetByManagerIdAsync(long managerId)
        {
            return await _dbSet
                .Where(x => x.ManagerId == managerId && !x.IsDeleted)
                .Include(x => x.Employee)
                .OrderByDescending(x => x.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<OneOnOneMeeting>> GetUpcomingAsync()
        {
            return await _dbSet
                .Where(x => !x.IsDeleted && x.Status == Scheduled)
                .Include(x => x.Employee)
                .Include(x => x.Manager)
                .OrderBy(x => x.ScheduledDate)
                .ToListAsync();
        }
    }
}