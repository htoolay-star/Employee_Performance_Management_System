using EPMS.Domain.Data;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Interface.Irepo.Auth;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Repository.Auth
{
    public class UserRefreshTokenRepository : GenericRepository<UserRefreshToken>, IUserRefreshTokenRepository
    {
        public UserRefreshTokenRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<UserRefreshToken?> GetByTokenWithUserAsync(string token, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(t => t.User)
                    .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(t => t.Token == token, cancellationToken);
        }
    }
}
