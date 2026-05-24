using EPMS.Domain.Entities.Auth;

namespace EPMS.Domain.Interface.Irepo.Auth;

public interface IPasswordResetRequestRepository
{
    Task<PasswordResetRequest?> GetByIdAsync(long id);
    Task<IEnumerable<PasswordResetRequest>> GetPendingAsync();
    void Add(PasswordResetRequest request);
}
