using EPMS.Domain.Data;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Interface.Irepo.Auth;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Auth;

public class PasswordResetOtpRepository : IPasswordResetOtpRepository
{
    private readonly AppDbContext _context;

    public PasswordResetOtpRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PasswordResetOtp?> GetValidOtpAsync(string email, string otp)
    {
        return await _context.Set<PasswordResetOtp>()
            .Where(o => o.Email == email && o.Otp == otp && !o.IsUsed && o.ExpiresAt > DateTimeOffset.UtcNow)
            .FirstOrDefaultAsync();
    }

    public void Add(PasswordResetOtp otp)
    {
        _context.Set<PasswordResetOtp>().Add(otp);
    }
}
