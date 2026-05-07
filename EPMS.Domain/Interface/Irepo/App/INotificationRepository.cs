using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.App;

namespace EPMS.Domain.Interface.Irepo.App;

public interface INotificationRepository : IGenericRepository<Notification>
{
    Task<IEnumerable<Notification>> GetByUserIdAsync(long userId);
    Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(long userId);
}