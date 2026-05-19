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

        public async Task<Appraisal?> GetAppraisalWithDetailsAsync(long id)
        {
            return await _dbSet
                .Include(a => a.Employee)
                    .ThenInclude(e => e.Employment)
                        .ThenInclude(emp => emp.Position)
                .Include(a => a.Employee)
                    .ThenInclude(e => e.Employment)
                        .ThenInclude(emp => emp.Department)
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
    }
}