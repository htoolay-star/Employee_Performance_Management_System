using EPMS.Application.Interfaces.Performance;
using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Infrastructure.Repositories.Performance
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
                .Include(a => a.Appraiser)
                .Include(a => a.Cycle)
                .Include(a => a.Details)
                    .ThenInclude(d => d.Question)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Appraisal>> GetEmployeeAppraisalsAsync(long employeeId, int cycleId)
        {
            return await _dbSet
                .Where(a => a.EmployeeId == employeeId && a.CycleId == cycleId)
                .Include(a => a.Details)
                .ToListAsync();
        }

        public async Task<bool> HasAlreadySubmittedAsync(long employeeId, long appraiserId, int cycleId, string role)
        {
            return await _dbSet.AnyAsync(a =>
                a.EmployeeId == employeeId &&
                a.AppraiserId == appraiserId &&
                a.CycleId == cycleId &&
                a.EvaluatorRole == role);
        }
    }
}