using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Auth;

namespace EPMS.Domain.Interface.Irepo.Auth
{
    public interface IUserRefreshTokenRepository : IGenericRepository<UserRefreshToken>
    {
        Task<UserRefreshToken?> GetByTokenWithUserAsync(string token, CancellationToken cancellationToken = default);
    }
}
