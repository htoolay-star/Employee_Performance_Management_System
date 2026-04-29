using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;

namespace EPMS.Domain.Interface.Irepo.Performance
{
    public interface IAppraisalRepository : IGenericRepository<Appraisal>
    {
        Task<Appraisal?> GetAppraisalWithDetailsAsync(long id);
        Task<IEnumerable<Appraisal>> GetEmployeeAppraisalsAsync(long employeeId, int cycleId);
        Task<bool> HasAlreadySubmittedAsync(long employeeId, long appraiserId, int cycleId, string role);
    }
}