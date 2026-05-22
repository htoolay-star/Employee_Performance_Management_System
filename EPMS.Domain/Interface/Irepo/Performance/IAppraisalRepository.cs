using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;

namespace EPMS.Domain.Interface.Irepo.Performance
{
    public interface IAppraisalRepository : IGenericRepository<Appraisal>
    {
        Task<Appraisal?> GetAppraisalWithDetailsAsync(long id);
        Task<IEnumerable<Appraisal>> GetEmployeeAppraisalsAsync(long employeeId, int cycleId);
        Task<bool> ExistsByEmployeeAndCycleAsync(long employeeId, long cycleId);
        Task<bool> ExistsByEntityAndCycleAsync(string entityType, long entityId, long cycleId);
        Task<IEnumerable<Appraisal>> GetByEntityAndCycleAsync(string entityType, long entityId, long cycleId);
        Task<IEnumerable<Appraisal>> GetAppraisalsByCycleAsync(long cycleId);
        Task<IEnumerable<Appraisal>> GetByManagerReviewerIdAsync(long managerReviewerId);
        Task<IEnumerable<Appraisal>> GetByNoDirectManagerAsync();
    }
}