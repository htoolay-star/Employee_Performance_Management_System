using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;

namespace EPMS.Domain.Interface.Irepo.Performance
{
    public interface IContinuousFeedbackRepository : IGenericRepository<ContinuousFeedback>
    {
        Task<IEnumerable<ContinuousFeedback>> GetByEmployeeIdAsync(long employeeId);
        Task<IEnumerable<ContinuousFeedback>> GetGivenByUserIdAsync(long userId);
    }
}