using EPMS.Domain.Data;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Interface.Irepo.Auth;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Auth
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<bool> ExistsAsync(string email) =>
            await _dbSet.AnyAsync(u => u.Email == email);

        public override async Task<User?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            if (id is not long userId)
                return null;

            return await _dbSet
                .Include(u => u.Role)
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, bool trackChanges = false)
        {
            var query = _dbSet
                .Include(u => u.Role)
                .Include(u => u.Profile)
                .Where(u => u.Email == email && u.IsActive);

            if (!trackChanges)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync();
        }
    }
}
