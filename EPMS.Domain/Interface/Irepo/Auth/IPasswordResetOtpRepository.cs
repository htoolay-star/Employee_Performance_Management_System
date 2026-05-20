using EPMS.Domain.Entities.Auth;

namespace EPMS.Domain.Interface.Irepo.Auth;

public interface IPasswordResetOtpRepository
{
    Task<PasswordResetOtp?> GetValidOtpAsync(string email, string otp);
    void Add(PasswordResetOtp otp);
}
