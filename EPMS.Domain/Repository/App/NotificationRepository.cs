using EPMS.Domain.Data;
using EPMS.Domain.Entities.App;
using EPMS.Domain.Interface.Irepo.App;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.App;

public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
{
    public NotificationRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Notification>> GetByUserIdAsync(long userId)
    {
        return await _dbSet
            .Where(n => n.ToUserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(long userId)
    {
        return await _dbSet
            .Where(n => n.ToUserId == userId && !n.IsRead)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    public async Task MarkAllAsReadAsync(long userId)
    {
        var now = DateTimeOffset.UtcNow;
        await _dbSet
            .Where(n => n.ToUserId == userId && !n.IsRead)
            .ExecuteUpdateAsync(s => s
                .SetProperty(n => n.IsRead, true)
                .SetProperty(n => n.ReadAt, now));
    }
}