using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;
using static EPMS.Shared.Constants.PIPStatuses;

namespace EPMS.Domain.Repository.Performance
{
    public class PIPRepository : GenericRepository<PIP>, IPIPRepository
    {
        public PIPRepository(AppDbContext context) : base(context) { }

        public override async Task<IEnumerable<PIP>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(x => x.Employee)
                .Include(x => x.Manager)
                .OrderByDescending(x => x.StartDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<PIP>> GetByEmployeeIdAsync(long employeeId)
        {
            return await _dbSet
                .Where(x => x.EmployeeId == employeeId && !x.IsDeleted)
                .Include(x => x.Manager)
                .OrderByDescending(x => x.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<PIP>> GetByManagerIdAsync(long managerId)
        {
            return await _dbSet
                .Where(x => x.ManagerId == managerId && !x.IsDeleted)
                .Include(x => x.Employee)
                .OrderByDescending(x => x.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<PIP>> GetActivePIPsAsync()
        {
            return await _dbSet
                .Where(x => !x.IsDeleted && x.Status != Successful && x.Status != Failed)
                .Include(x => x.Employee)
                .Include(x => x.Manager)
                .OrderByDescending(x => x.StartDate)
                .ToListAsync();
        }
    }
}