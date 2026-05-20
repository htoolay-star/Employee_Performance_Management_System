using EPMS.Domain.Data;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Interface.Irepo.Auth;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Auth;

public class PasswordResetRequestRepository : IPasswordResetRequestRepository
{
    private readonly AppDbContext _context;

    public PasswordResetRequestRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PasswordResetRequest?> GetByIdAsync(long id)
    {
        return await _context.Set<PasswordResetRequest>()
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<PasswordResetRequest>> GetPendingAsync()
    {
        return await _context.Set<PasswordResetRequest>()
            .Include(r => r.User)
            .ThenInclude(u => u.Profile)
            .Where(r => r.Status == ResetRequestStatus.Pending)
            .OrderByDescending(r => r.RequestedAt)
            .ToListAsync();
    }

    public void Add(PasswordResetRequest request)
    {
        _context.Set<PasswordResetRequest>().Add(request);
    }
}
