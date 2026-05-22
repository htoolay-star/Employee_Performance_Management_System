using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Performance
{
    public class AppraisalRepository : GenericRepository<Appraisal>, IAppraisalRepository
    {
        public AppraisalRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Appraisal>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(a => a.Employee)
                .Include(a => a.Cycle)
                .Include(a => a.ManagerReviewer)
                .ToListAsync(cancellationToken);
        }

        public async Task<Appraisal?> GetAppraisalWithDetailsAsync(long id)
        {
            return await _dbSet
                .AsNoTracking()
                .AsSplitQuery()
                .Include(a => a.Employee)
                    .ThenInclude(e => e.Employment)
                        .ThenInclude(emp => emp.Position)
                .Include(a => a.Employee)
                    .ThenInclude(e => e.Employment)
                        .ThenInclude(emp => emp.Department)
                .Include(a => a.Employee)
                    .ThenInclude(e => e.Employment)
                        .ThenInclude(emp => emp.Team)
                .Include(a => a.Employee)
                    .ThenInclude(e => e.Employment)
                        .ThenInclude(emp => emp.DirectManager)
                .Include(a => a.ManagerReviewer)
                .Include(a => a.Cycle)
                .Include(a => a.Details)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Appraisal>> GetEmployeeAppraisalsAsync(long employeeId, int cycleId)
        {
            return await _dbSet
                .Where(a => a.EmployeeId == employeeId && a.CycleId == cycleId)
                .Include(a => a.Details)
                .ToListAsync();
        }

        public async Task<bool> ExistsByEmployeeAndCycleAsync(long employeeId, long cycleId)
        {
            return await _dbSet.AnyAsync(a =>
                a.EmployeeId == employeeId &&
                a.CycleId == cycleId);
        }

        public async Task<bool> ExistsByEntityAndCycleAsync(string entityType, long entityId, long cycleId)
        {
            return await _dbSet.AnyAsync(a =>
                a.EntityType == entityType &&
                a.EntityId == entityId &&
                a.CycleId == cycleId);
        }

        public async Task<IEnumerable<Appraisal>> GetByEntityAndCycleAsync(string entityType, long entityId, long cycleId)
        {
            return await _dbSet
                .Where(a => a.EntityType == entityType && a.EntityId == entityId && a.CycleId == cycleId)
                .Include(a => a.Details)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appraisal>> GetByManagerReviewerIdAsync(long managerReviewerId)
        {
            return await _dbSet
                .Where(a => a.ManagerReviewerId == managerReviewerId && !a.IsDeleted)
                .AsNoTracking()
                .Include(a => a.Employee)
                .Include(a => a.Cycle)
                .Include(a => a.ManagerReviewer)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appraisal>> GetAppraisalsByCycleAsync(long cycleId)
        {
            return await _dbSet
                .Where(a => a.CycleId == cycleId)
                .Include(a => a.Employee)
                .Include(a => a.Details)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appraisal>> GetByNoDirectManagerAsync()
        {
            return await _dbSet
                .Where(a => a.Employee != null
                    && a.Employee.Employment != null
                    && a.Employee.Employment.DirectManagerId == null
                    && !a.IsDeleted)
                .AsNoTracking()
                .Include(a => a.Employee)
                .Include(a => a.Cycle)
                .Include(a => a.ManagerReviewer)
                .ToListAsync();
        }
    }
}
