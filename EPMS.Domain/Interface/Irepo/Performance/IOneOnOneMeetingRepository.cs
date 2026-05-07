using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;

namespace EPMS.Domain.Interface.Irepo.Performance
{
    public interface IOneOnOneMeetingRepository : IGenericRepository<OneOnOneMeeting>
    {
        Task<IEnumerable<OneOnOneMeeting>> GetByEmployeeIdAsync(long employeeId);
        Task<IEnumerable<OneOnOneMeeting>> GetByManagerIdAsync(long managerId);
        Task<IEnumerable<OneOnOneMeeting>> GetUpcomingAsync();
    }
}